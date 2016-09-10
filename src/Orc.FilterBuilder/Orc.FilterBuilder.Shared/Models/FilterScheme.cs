// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterScheme.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder.Models
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;

    using Catel;
    using Catel.Data;
    using Catel.Runtime.Serialization;

    using Orc.FilterBuilder.Runtime.Serialization;

    /// <summary>
    ///     Filter conditions container. Provides functionalities to dynamically build a filtering
    ///     Condition Tree.
    /// </summary>
    [SerializerModifier(typeof(FilterSchemeSerializerModifier))]
    public class FilterScheme : ModelBase
    {
        #region Constants
        private static readonly Type DefaultTargetType = typeof(object);
        #endregion



        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterScheme"/> class.
        /// </summary>
        public FilterScheme() : this(DefaultTargetType) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterScheme" /> class.
        /// </summary>
        /// <param name="targetType">Filter target type.</param>
        public FilterScheme(Type targetType) : this(targetType, string.Empty) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterScheme" /> class.
        /// </summary>
        /// <param name="targetType">Filter target type.</param>
        /// <param name="title">Filter title.</param>
        public FilterScheme(Type targetType, string title)
        {
            Argument.IsNotNull(() => targetType);

            SuspendValidation = true;

            TargetType = targetType;
            Title = title;
            OptimizeTree = true;

            ConditionItems = new ObservableCollection<ConditionTreeItem>();
            this.CreateRootNode();

            IsExpressionValid = CheckExpressionValid();
        }
        #endregion



        #region Properties
        /// <summary>
        ///     Filter title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     If true, will attempt to optimize tree when a <see cref="ConditionGroup" /> value is
        ///     changed. Optimization also occurs when changing this value to true.
        /// </summary>
        /// <remarks>Defaults to true.</remarks>
        [ExcludeFromSerialization]
        public bool OptimizeTree { get; set; } // TODO : Optimize away

        /// <summary>
        ///     Root <see cref="ConditionTreeItem" /> item, of type <see cref="ConditionGroup" />.
        /// </summary>
        /// <remarks>Defaults to <see cref="ConditionGroupType.And" /></remarks>
        [ExcludeFromSerialization]
        public ConditionTreeItem RootItem => ConditionItems?.FirstOrDefault();

        /// <summary>
        ///     Filter conditions.
        /// </summary>
        public ObservableCollection<ConditionTreeItem> ConditionItems { get; protected set; }

        /// <summary>
        ///     Whether filter expression is valid.
        /// </summary>
        public bool IsExpressionValid { get; protected set; }

        /// <summary>
        ///     Context object.
        /// </summary>
        [ExcludeFromSerialization]
        public object Scope { get; set; }

        /// <summary>
        ///     Target type for current filter.
        /// </summary>
        public Type TargetType { get; set; }
        #endregion



        #region Methods
        /// <summary>
        ///     Update current filter with otherScheme datas :
        ///     - Title
        ///     - Condition items
        /// </summary>
        /// <param name="otherScheme">The other scheme.</param>
        public void Update(FilterScheme otherScheme)
        {
            Argument.IsNotNull(() => otherScheme);

            Title = otherScheme.Title;
            ConditionItems.Clear();
            ConditionItems.Add(otherScheme.RootItem);

            IsExpressionValid = CheckExpressionValid();

            Updated.SafeInvoke(this);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var filterScheme = obj as FilterScheme;

            if (filterScheme == null)
                return false;

            return string.Equals(filterScheme.Title, Title);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Title);

            var rootString = RootItem.ToString();

            if (rootString.StartsWith("((") && rootString.EndsWith("))"))
            {
                rootString = rootString.Substring(1, rootString.Length - 2);
            }

            if (!string.IsNullOrEmpty(rootString))
            {
                stringBuilder.AppendLine();
                stringBuilder.Append(rootString);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Called when FilterScheme is deserialized
        /// </summary>
        protected override void OnDeserialized()
        {
            base.OnDeserialized();

            SubscribeToEvents();
        }

        /// <summary>
        ///     Checks whether filter expression is valid.
        /// </summary>
        /// <returns></returns>
        protected bool CheckExpressionValid()
        {
            return RootItem == null || CheckExpressionValid(RootItem);
        }

        /// <summary>
        ///     Checks whether given expression is valid and recurse.
        /// </summary>
        /// <param name="item">Current recurse item</param>
        /// <returns></returns>
        protected bool CheckExpressionValid(ConditionTreeItem item)
        {
            return item != null && item.IsValid && (item.Items == null || item.Items.All(CheckExpressionValid));
        }

        /// <summary>
        ///   Subscribe to <see cref="ConditionItems"/> change event to validate FilterScheme on
        ///   item modification.
        /// </summary>
        protected void SubscribeToEvents()
        {
            var items = ConditionItems;

            if (items != null)
            {
                items.CollectionChanged += OnConditionItemsCollectionChanged;

                foreach (var item in items)
                    item.Updated += OnItemUpdated;
            }
        }

        /// <summary>
        ///   Called when <see cref="ConditionItems"/> reference changes.
        /// </summary>
        private void OnConditionItemsChanged()
        {
            SubscribeToEvents();
        }

        private void OnConditionItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (var item in e.OldItems)
                    ((ConditionTreeItem)item).Updated -= OnItemUpdated;

            var newCollection = e.Action == NotifyCollectionChangedAction.Reset ? (IList)sender : e.NewItems;

            if (newCollection != null)
                foreach (var item in newCollection)
                    ((ConditionTreeItem)item).Updated += OnItemUpdated;
        }

        private void OnItemUpdated(object sender, EventArgs e)
        {
            IsExpressionValid = CheckExpressionValid();

            Updated.SafeInvoke(this);
        }
        #endregion



        #region Events
        /// <summary>
        ///     Occurs when an item in the expression tree is updated.
        /// </summary>
        public event EventHandler<EventArgs> Updated;
        #endregion
    }
}
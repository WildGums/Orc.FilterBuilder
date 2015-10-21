// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterScheme.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
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
    using Runtime.Serialization;

    [SerializerModifier(typeof(FilterSchemeSerializerModifier))]
    public class FilterScheme : ModelBase
    {
        #region Constructors
        public FilterScheme()
            : this(typeof(object))
        {
        }

        public FilterScheme(Type targetType)
            : this(targetType, string.Empty)
        {
        }

        public FilterScheme(Type targetType, string title)
            : this(targetType, title, new ConditionGroup())
        {
        }

        public FilterScheme(Type targetType, string title, ConditionTreeItem root)
        {
            Argument.IsNotNull(() => targetType);
            Argument.IsNotNull(() => title);
            Argument.IsNotNull(() => root);

            SuspendValidation = true;

            TargetType = targetType;
            Title = title;
            ConditionItems = new ObservableCollection<ConditionTreeItem>();
            ConditionItems.Add(root);
        }
        #endregion

        #region Properties
        public Type TargetType { get; private set; }

        public string Title { get; set; }

        [ExcludeFromSerialization]
        public ConditionTreeItem Root
        {
            get { return ConditionItems.FirstOrDefault(); }
        }

        [ExcludeFromSerialization]
        public object Tag { get; set; }

        public ObservableCollection<ConditionTreeItem> ConditionItems { get; private set; }
        #endregion

        public event EventHandler<EventArgs> Updated;

        #region Methods
        private void OnConditionItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((ConditionTreeItem)item).Updated -= OnConditionUpdated;
                }
            }

            var newCollection = (e.Action == NotifyCollectionChangedAction.Reset) ? (IList)sender : e.NewItems;
            if (newCollection != null)
            {
                foreach (var item in newCollection)
                {
                    ((ConditionTreeItem)item).Updated += OnConditionUpdated;
                }
            }
        }

        private void OnConditionItemsChanged()
        {
            SubscribeToEvents();
        }

        protected override void OnDeserialized()
        {
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            var items = ConditionItems;
            if (items != null)
            {
                items.CollectionChanged += OnConditionItemsCollectionChanged;
                foreach (var item in items)
                {
                    item.Updated += OnConditionUpdated;
                }
            }
        }

        private void OnConditionUpdated(object sender, EventArgs e)
        {
            RaiseUpdated();
        }

        public bool CalculateResult(object entity)
        {
            Argument.IsNotNull(() => entity);

            var root = Root;
            if (root != null)
            {
                return root.CalculateResult(entity);
            }

            return true;
        }

        public void Update(FilterScheme otherScheme)
        {
            Argument.IsNotNull(() => otherScheme);

            Title = otherScheme.Title;
            ConditionItems.Clear();
            ConditionItems.Add(otherScheme.Root);

            RaiseUpdated();
        }

        protected void RaiseUpdated()
        {
            Updated.SafeInvoke(this);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Title);

            var rootString = Root.ToString();
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

        public override bool Equals(object obj)
        {
            var filterScheme = obj as FilterScheme;
            if (filterScheme == null)
            {
                return false;
            }

            return string.Equals(filterScheme.Title, Title);
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
        #endregion
    }
}
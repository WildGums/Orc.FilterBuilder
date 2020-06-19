// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterScheme.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Runtime.Serialization;
    using Runtime.Serialization;

    [SerializerModifier(typeof(FilterSchemeSerializerModifier))]
    public class FilterScheme : ModelBase
    {
        private static readonly Type DefaultTargetType = typeof(object);
        private object _scope;

        #region Constructors
        public FilterScheme()
            : this(DefaultTargetType)
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

            TargetType = targetType;
            Title = title;
            ConditionItems = new ObservableCollection<ConditionTreeItem>();
            ConditionItems.Add(root);
            CanEdit = true;
            CanDelete = true;
        }
        #endregion

        #region Properties
        [IncludeInSerialization]
        public Type TargetType { get; private set; }

        public string Title { get; set; }

        public string FilterGroup { get; set; }

        [ExcludeFromSerialization]
        public bool CanEdit { get; set; }

        [ExcludeFromSerialization]
        public bool CanDelete { get; set; }

        [ExcludeFromSerialization]
        public ConditionTreeItem Root
        {
            get { return ConditionItems.FirstOrDefault(); }
        }

        [ExcludeFromSerialization]
        public object Scope
        {
            get { return _scope; }
            set
            {
                if (!ObjectHelper.AreEqual(_scope, value))
                {
                    _scope = value;

                    RaisePropertyChanged(nameof(Scope));

                    var reflectionService = this.GetServiceLocator().ResolveType<IReflectionService>(_scope);
                    if (reflectionService != null)
                    {
                        this.EnsureIntegrity(reflectionService);
                    }
                }
            }
        }

        public bool HasInvalidConditionItems { get; private set; }

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
            base.OnDeserialized();

            SubscribeToEvents();
        }

        private void CheckForInvalidItems()
        {
            HasInvalidConditionItems = (ConditionItems != null && ConditionItems.Count > 0 && CountInvalidItems(Root) > 0);
        }

        private int CountInvalidItems(ConditionTreeItem conditionTreeItem)
        {
            var items = conditionTreeItem?.Items;
            if (items is null || items.Count == 0)
            {
                return conditionTreeItem?.IsValid??true ? 0 : 1;
            }

            var invalidCount = 0;
            foreach (var item in items)
            {
                invalidCount += CountInvalidItems(item);
            }

            invalidCount += conditionTreeItem.IsValid ? 0 : 1;

            return invalidCount;
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
            CheckForInvalidItems();

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

            CheckForInvalidItems();

            RaiseUpdated();
        }

        protected void RaiseUpdated()
        {
            Updated?.Invoke(this, EventArgs.Empty);
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
            if (filterScheme is null)
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

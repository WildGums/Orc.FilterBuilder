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
    using Catel;
    using Catel.Data;
    using Catel.Runtime.Serialization;
    using Runtime.Serialization;

    [SerializerModifier(typeof(FilterSchemeSerializerModifier))]
    public class FilterScheme : ModelBase
    {
        #region Constructors
        // Parameterless constructor for deserialization purposes
        public FilterScheme()
        {
        }

        public FilterScheme(IMetadataProvider targetDataDescriptor)
            : this(targetDataDescriptor, string.Empty)
        {
        }

        public FilterScheme(IMetadataProvider targetDataDescriptor, string title)
            : this(targetDataDescriptor, title, new ConditionGroup())
        {
        }

        public FilterScheme(IMetadataProvider targetDataDescriptor, string title, ConditionTreeItem root)
        {
            Argument.IsNotNull(() => targetDataDescriptor);
            Argument.IsNotNull(() => title);
            Argument.IsNotNull(() => root);

            SuspendValidation = true;

            TargetDataDescriptor = targetDataDescriptor;
            Title = title;
            ConditionItems = new ObservableCollection<ConditionTreeItem>();
            ConditionItems.Add(root);
        }
        #endregion

        #region Properties
        public IMetadataProvider TargetDataDescriptor { get; private set; }

        public string Title { get; set; }

        [ExcludeFromSerialization]
        public ConditionTreeItem Root
        {
            get { return ConditionItems.FirstOrDefault(); }
        }

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
            Updated.SafeInvoke(this);
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
        }
        #endregion
    }
}
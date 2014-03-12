// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionTreeItem.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using Catel;
    using Catel.Data;
    using Catel.Runtime.Serialization;

    public abstract class ConditionTreeItem : ModelBase
    {
        #region Constructors
        protected ConditionTreeItem()
        {
            Items = new ObservableCollection<ConditionTreeItem>();
        }
        #endregion

        #region Properties
        [ExcludeFromSerialization]
        public ConditionTreeItem Parent { get; set; }

        public ObservableCollection<ConditionTreeItem> Items { get; private set; }
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

        protected override void OnDeserialized()
        {
            SubscribeToEvents();
        }

        private void OnItemsChanged()
        {
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            var items = Items;
            if (items != null)
            {
                items.CollectionChanged += OnConditionItemsCollectionChanged;
                foreach (var item in items)
                {
                    item.Updated += OnConditionUpdated;
                }
            }
        }

        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            RaiseUpdated();
        }

        protected void RaiseUpdated()
        {
            Updated.SafeInvoke(this);
        }

        private void OnConditionUpdated(object sender, EventArgs e)
        {
            RaiseUpdated();
        }

        public abstract bool CalculateResult(object entity);
        #endregion
    }
}
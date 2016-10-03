// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionTreeItem.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq.Expressions;

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

        [ExcludeFromSerialization]
        [ExcludeFromValidation]
        public bool IsValid { get; private set; } = true;

        public ObservableCollection<ConditionTreeItem> Items { get; private set; }
        #endregion



        #region Methods
        private void OnConditionItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var conditionTreeItem = (ConditionTreeItem)item;

                    if (ReferenceEquals(conditionTreeItem, this))
                    {
                        conditionTreeItem.Parent = null;
                    }

                    conditionTreeItem.Updated -= OnConditionUpdated;
                }
            }

            var newCollection = (e.Action == NotifyCollectionChangedAction.Reset) ? (IList)sender : e.NewItems;
            if (newCollection != null)
            {
                foreach (var item in newCollection)
                {
                    var conditionTreeItem = (ConditionTreeItem)item;

                    conditionTreeItem.Parent = this;
                    conditionTreeItem.Updated += OnConditionUpdated;
                }
            }
        }

        protected override void OnDeserialized()
        {
            base.OnDeserialized();

            SubscribeToEvents();

            foreach (var item in Items)
            {
                item.Parent = this;
            }
        }

        protected override void OnValidated(IValidationContext validationContext)
        {
            base.OnValidated(validationContext);

            IsValid = !validationContext.HasErrors;
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

        /// <summary>
        ///   Converts <see cref="ConditionTreeItem"/> to a LINQ <see cref="Expression"/>
        /// </summary>
        /// <param name="parameterExpr">LINQ ParameterExpression.</param>
        /// <returns>LINQ Expression.</returns>
        public abstract Expression ToLinqExpression(Expression parameterExpr);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion



        public event EventHandler<EventArgs> Updated;
    }
}
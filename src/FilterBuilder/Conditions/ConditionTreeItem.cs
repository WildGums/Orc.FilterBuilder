// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionTreeItem.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System.Collections.ObjectModel;
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

        public ObservableCollection<ConditionTreeItem> Items { get; set; }
        #endregion

        #region Methods
        public abstract bool CalculateResult(object entity);

        public ConditionTreeItem Copy()
        {
            var copiedItem = CopyPlainItem();

            foreach (var childItem in Items)
            {
                var copiedChild = childItem.Copy();
                copiedItem.Items.Add(copiedChild);
                copiedChild.Parent = copiedItem;
            }

            return copiedItem;
        }

        protected abstract ConditionTreeItem CopyPlainItem();
        #endregion
    }
}
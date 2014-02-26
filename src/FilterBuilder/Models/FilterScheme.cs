// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterScheme.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel;
    using Catel.Data;
    using Catel.Runtime.Serialization;
    using Orc.FilterBuilder.Runtime.Serialization;

    [SerializerModifier(typeof(FilterSchemeSerializerModifier))]
    public class FilterScheme : ModelBase
    {
        #region Constructors
        public FilterScheme()
            : this(typeof(object))
        {
        }

        public FilterScheme(Type targetType)
            : this(targetType, string.Empty, new ConditionGroup())
        {
        }

        public FilterScheme(Type targetType, string title, ConditionTreeItem root)
        {
            Argument.IsNotNull("targetType", targetType);
            Argument.IsNotNull("title", title);
            Argument.IsNotNull("root", root);

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
            get { return ConditionItems.First(); }
        }

        public ObservableCollection<ConditionTreeItem> ConditionItems { get; private set; }
        #endregion

        #region Methods
        public bool CalculateResult(object entity)
        {
            Argument.IsNotNull(() => entity);

            return Root.CalculateResult(entity);
        }

        public FilterScheme Copy()
        {
            return new FilterScheme(TargetType, Title, Root.Copy());
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
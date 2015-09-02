// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionTreeItemExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using Catel;

    public static class ConditionTreeItemExtensions
    {
        public static bool IsRoot(this ConditionTreeItem item)
        {
            Argument.IsNotNull(() => item);

            return item.Parent == null;
        }
    }
}
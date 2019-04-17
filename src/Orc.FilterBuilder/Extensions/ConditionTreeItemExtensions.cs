// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionTreeItemExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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

            return item.Parent is null;
        }
    }
}

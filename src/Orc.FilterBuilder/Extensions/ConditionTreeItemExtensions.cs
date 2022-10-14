namespace Orc.FilterBuilder
{
    using System;

    public static class ConditionTreeItemExtensions
    {
        public static bool IsRoot(this ConditionTreeItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            return item.Parent is null;
        }
    }
}

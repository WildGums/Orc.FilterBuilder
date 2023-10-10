namespace Orc.FilterBuilder;

using System.Windows.Controls;
using System.Windows.Media;

public static class TreeViewItemExtensions
{
    public static int GetDepth(this TreeViewItem item)
    {
        var parent = GetParent(item);
        if (parent is not null)
        {
            return GetDepth(parent) + 1;
        }

        return 0;
    }

    private static TreeViewItem? GetParent(TreeViewItem item)
    {
        var parent = VisualTreeHelper.GetParent(item);

        while (parent is not (TreeViewItem or TreeView))
        {
            if (parent is null)
            {
                return null;
            }

            parent = VisualTreeHelper.GetParent(parent);
        }

        return parent as TreeViewItem;
    }
}

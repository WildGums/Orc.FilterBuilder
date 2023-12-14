namespace Orc.FilterBuilder.Behaviors;

using System.Windows;
using System.Windows.Controls;
using Catel.Windows.Interactivity;

public class DisableSelectionInTreeView : BehaviorBase<TreeView>
{
    protected override void OnAssociatedObjectLoaded()
    {
        base.OnAssociatedObjectLoaded();

        var treeView = AssociatedObject;
        treeView.SelectedItemChanged += OnSelectedItemChanged;
    }

    protected override void OnAssociatedObjectUnloaded()
    {
        var treeView = AssociatedObject;
        treeView.SelectedItemChanged -= OnSelectedItemChanged;

        base.OnAssociatedObjectUnloaded();
    }

    private void OnSelectedItemChanged(object? sender, RoutedPropertyChangedEventArgs<object?> e)
    {
        var treeView = AssociatedObject;
        var selectedItem = treeView.SelectedItem;
        if (selectedItem is not null)
        {
            ClearTreeViewItemsControlSelection(treeView.Items, treeView.ItemContainerGenerator);
        }
    }

    private static void ClearTreeViewItemsControlSelection(ItemCollection ic, ItemContainerGenerator icg)
    {
        for (var i = 0; i < ic.Count; i++)
        {
            if (icg.ContainerFromIndex(i) is not TreeViewItem tvi)
            {
                continue;
            }

            if (tvi.IsSelected)
            {
                tvi.SetCurrentValue(TreeViewItem.IsSelectedProperty, false);
            }

            ClearTreeViewItemsControlSelection(tvi.Items, tvi.ItemContainerGenerator);
        }
    }
}

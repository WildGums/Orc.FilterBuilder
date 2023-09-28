﻿namespace Orc.FilterBuilder.Behaviors;

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
        if ((ic is not null) && (icg is not null))
        {
            for (var i = 0; i < ic.Count; i++)
            {
                var tvi = icg.ContainerFromIndex(i) as TreeViewItem;
                if (tvi is not null)
                {
                    if (tvi.IsSelected)
                    {
                        tvi.SetCurrentValue(TreeViewItem.IsSelectedProperty, false);
                    }

                    ClearTreeViewItemsControlSelection(tvi.Items, tvi.ItemContainerGenerator);
                }
            }
        }
    }
}
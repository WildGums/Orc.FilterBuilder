// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisableSelectionInTreeView.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Behaviors
{
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

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = AssociatedObject;
            var selectedItem = treeView.SelectedItem;
            if (selectedItem != null)
            {
                ClearTreeViewItemsControlSelection(treeView.Items, treeView.ItemContainerGenerator);
            }
        }

        private static void ClearTreeViewItemsControlSelection(ItemCollection ic, ItemContainerGenerator icg)
        {
            if ((ic != null) && (icg != null))
            {
                for (var i = 0; i < ic.Count; i++)
                {
                    var tvi = icg.ContainerFromIndex(i) as TreeViewItem;
                    if (tvi != null)
                    {
                        if (tvi.IsSelected)
                        {
                            tvi.IsSelected = false;
                        }

                        ClearTreeViewItemsControlSelection(tvi.Items, tvi.ItemContainerGenerator);
                    }
                }
            }
        }
    }
}
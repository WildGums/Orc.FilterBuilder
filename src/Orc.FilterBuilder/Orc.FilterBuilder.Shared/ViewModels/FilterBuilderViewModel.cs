// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBuilderViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Catel.IoC;
    using Catel.MVVM;

    using GongSolutions.Wpf.DragDrop;

    using Orc.FilterBuilder.Models;
    using Orc.FilterBuilder.Services;

    /// <summary>
    ///   View Model for generic FilterBuilder
    /// </summary>
    /// <seealso cref="Catel.MVVM.ViewModelBase" />
    public class FilterBuilderViewModel : ViewModelBase, IDragSource, IDropTarget
    {
        #region Fields
        private readonly IReflectionService _reflectionService;
        private readonly bool _isEditing = false;
        private readonly DefaultDragHandler _defaultDragHandler;
        #endregion



        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterBuilderViewModel" /> class.
        /// </summary>
        /// <param name="targetType">Filter target type.</param>
        /// <param name="reflectionService">The reflection service.</param>
        public FilterBuilderViewModel(Type targetType, IReflectionService reflectionService) : this(TypeFactory.Default.CreateInstanceWithParameters<FilterScheme>(targetType), reflectionService)
        {
            _isEditing = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterBuilderViewModel" /> class.
        /// </summary>
        /// <param name="scheme">Filter scheme.</param>
        /// <param name="reflectionService">The reflection service.</param>
        public FilterBuilderViewModel(FilterScheme scheme, IReflectionService reflectionService) : base(false)
        {
            _reflectionService = reflectionService;
            _defaultDragHandler = new DefaultDragHandler();

            CurrentFilter = scheme;

            DeleteCommand = new Command<ConditionTreeItem>(OnDeleteCommandExecute, OnDeleteCommandCanExecute);
            AddAndConditionCommand = new Command<ConditionTreeItem>(OnAddAndConditionCommandExecute);
            AddOrConditionCommand = new Command<ConditionTreeItem>(OnAddOrConditionCommandExecute);
        }
        #endregion



        #region Properties
        /// <summary>
        ///   Current FilterScheme
        /// </summary>
        [Model]
        public FilterScheme CurrentFilter { get; set; }

        /// <summary>
        ///   Filter items to display.
        /// </summary>
        [ViewModelToModel]
        public ObservableCollection<ConditionTreeItem> ConditionItems { get; set; }

        [ViewModelToModel]
        public bool IsExpressionValid { get; set; }

        /// <summary>
        ///   Target type properties, used to display items in property combobox.
        /// </summary>
        public IEnumerable<IPropertyMetadata> TargetProperties { get; set; }

        /// <summary>
        ///   Command for adding a new And condition.
        /// </summary>
        public ICommand AddAndConditionCommand { get; set; }

        /// <summary>
        ///   Command for adding a new or condition.
        /// </summary>
        public ICommand AddOrConditionCommand { get; set; }

        /// <summary>
        ///   Command for removing a given item.
        /// </summary>
        public ICommand DeleteCommand { get; set; }
        #endregion



        #region IDragSource Members
        public void StartDrag(IDragInfo dragInfo)
        {
            _defaultDragHandler.StartDrag(dragInfo);
        }

        public bool CanStartDrag(IDragInfo dragInfo)
        {
            return (ConditionTreeItem)dragInfo.SourceItem != CurrentFilter.Root;
        }

        public void Dropped(IDropInfo dropInfo)
        {
            _defaultDragHandler.Dropped(dropInfo);
        }

        public void DragCancelled()
        {
            _defaultDragHandler.DragCancelled();
        }

        public bool TryCatchOccurredException(Exception exception)
        {
            return _defaultDragHandler.TryCatchOccurredException(exception);
        }
        #endregion



        #region IDropTarget Members
        public void DragOver(IDropInfo dropInfo)
        {
            if (DefaultDropHandler.CanAcceptData(dropInfo))
            {
                // Check whether we are are moving/copying, or inserting
                var isTreeViewItem = dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.TargetItemCenter)
                    && dropInfo.VisualTargetItem is TreeViewItem;
                var copyData = (dropInfo.DragInfo.DragDropCopyKeyState != default(DragDropKeyStates)) && dropInfo.KeyStates.HasFlag(dropInfo.DragInfo.DragDropCopyKeyState);

                // default should always the move action/effect
                dropInfo.Effects = copyData ? DragDropEffects.Copy : DragDropEffects.Move;

                // Can only move/copy to a ConditionGroup
                if (isTreeViewItem && IsTargetConditionGroup(dropInfo))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                }
                else
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                }
            }
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException", Justification = "Types are known")]
        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo?.DragInfo == null)
            {
                return;
            }

            // Get drag/drop targets
            ConditionTreeItem dragSource = dropInfo.DragInfo.SourceItem as ConditionTreeItem;
            ConditionTreeItem dropTarget = dropInfo.TargetItem as ConditionTreeItem;

            var insertIndex = dropInfo.InsertIndex != dropInfo.UnfilteredInsertIndex ? dropInfo.UnfilteredInsertIndex : dropInfo.InsertIndex;
            var itemsControl = dropInfo.VisualTarget as ItemsControl;

            // Adjust insert index depending on drop target if moving (copying doesn't involve removing drag source).
            var copyData = (dropInfo.DragInfo.DragDropCopyKeyState != default(DragDropKeyStates)) && dropInfo.KeyStates.HasFlag(dropInfo.DragInfo.DragDropCopyKeyState);

            var sourceParent = dragSource.Parent;

            if (!copyData)
            {
                var trueDropTarget = dropTarget is PropertyExpression ? dropTarget.Parent : dropTarget;

                if (sourceParent == trueDropTarget)
                {
                    insertIndex = Math.Max(0, insertIndex - 1);

                    if (insertIndex == sourceParent.Items.IndexOf(dragSource))
                    {
                        return;
                    }
                }

                sourceParent.Items.Remove(dragSource);
            }

            // If copying, clone data
            if (copyData)
            {
                dragSource = (ConditionTreeItem)dragSource.Clone();
            }

            // Copy or move
            if (dropTarget is ConditionGroup)
            {
                dropTarget.Items.Insert(insertIndex, dragSource);
            }
            else if (dropTarget.Parent != null)
            {
                dropTarget.Parent.Items.Insert(insertIndex, dragSource);
            }

            CurrentFilter.OptimizeTree();
        }

        private bool IsTargetConditionGroup(IDropInfo dropInfo)
        {
            return dropInfo.TargetItem is ConditionGroup;
        }
        #endregion



        #region Methods
        /// <inheritdoc />
        protected override async Task InitializeAsync()
        {
            TargetProperties = (await _reflectionService.GetInstancePropertiesAsync(CurrentFilter.TargetType).ConfigureAwait(false)).Properties;

            if (!_isEditing)
                CurrentFilter.CreateRootProperty(TargetProperties);
        }

        private void OnAddAndConditionCommandExecute(ConditionTreeItem item)
        {
            CurrentFilter.Add(item, ConditionGroupType.And, TargetProperties);
            RaisePropertyChanged(() => DeleteCommand);
        }

        private void OnAddOrConditionCommandExecute(ConditionTreeItem item)
        {
            CurrentFilter.Add(item, ConditionGroupType.Or, TargetProperties);
            RaisePropertyChanged(() => DeleteCommand);
        }

        private bool OnDeleteCommandCanExecute(ConditionTreeItem item)
        {
            return CurrentFilter.CanRemove(item);
        }

        private void OnDeleteCommandExecute(ConditionTreeItem item)
        {
            CurrentFilter.Remove(item);
            RaisePropertyChanged(() => DeleteCommand);
        }
        #endregion
    }
}
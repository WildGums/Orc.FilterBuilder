// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBuilderControl.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Views
{
    using System.Collections;
    using System.Data;
    using System.Windows;
    using Catel.MVVM.Views;

    public partial class FilterBuilderControl
    {
        #region Constructors
        public FilterBuilderControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        public static readonly DependencyProperty RawCollectionProperty = DependencyProperty.Register("RawCollection", typeof(IEnumerable),
            typeof(FilterBuilderControl));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public IEnumerable RawCollection
        {
            get { return (IEnumerable)GetValue(RawCollectionProperty); }
            set { SetValue(RawCollectionProperty, value); }
        }

        public static readonly DependencyProperty FilteredCollectionProperty = DependencyProperty.Register("FilteredCollection", typeof(IList),
            typeof(FilterBuilderControl));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public IList FilteredCollection
        {
            get { return (IList)GetValue(FilteredCollectionProperty); }
            set { SetValue(FilteredCollectionProperty, value); }
        }

        public static readonly DependencyProperty AutoApplyFilterProperty =
            DependencyProperty.Register("AutoApplyFilter", typeof(bool), typeof(FilterBuilderControl), new PropertyMetadata(true));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool AutoApplyFilter
        {
            get { return (bool)GetValue(AutoApplyFilterProperty); }
            set { SetValue(AutoApplyFilterProperty, value); }
        }

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool AllowLivePreview
        {
            get { return (bool)GetValue(AllowLivePreviewProperty); }
            set { SetValue(AllowLivePreviewProperty, value); }
        }

        public static readonly DependencyProperty AllowLivePreviewProperty =
            DependencyProperty.Register("AllowLivePreview", typeof(bool), typeof(FilterBuilderControl), new PropertyMetadata(true));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool EnableAutoCompletion
        {
            get { return (bool)GetValue(EnableAutoCompletionProperty); }
            set { SetValue(EnableAutoCompletionProperty, value); }
        }

        public static readonly DependencyProperty EnableAutoCompletionProperty =
            DependencyProperty.Register("EnableAutoCompletion", typeof(bool), typeof(FilterBuilderControl), new PropertyMetadata(true));

        
        #endregion
    }
}
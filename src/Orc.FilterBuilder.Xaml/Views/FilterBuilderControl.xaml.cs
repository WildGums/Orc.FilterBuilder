﻿namespace Orc.FilterBuilder.Views
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Media;
    using Catel.MVVM.Views;
    using Orc.FilterBuilder.Models;
    using ViewModels;
    using PropertyMetadata = System.Windows.PropertyMetadata;

    public partial class FilterBuilderControl
    {
        #region Constructors
        static FilterBuilderControl()
        {
            typeof(FilterBuilderControl).AutoDetectViewPropertiesToSubscribe();
        }

        public FilterBuilderControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public IEnumerable RawCollection
        {
            get { return (IEnumerable)GetValue(RawCollectionProperty); }
            set { SetValue(RawCollectionProperty, value); }
        }

        public static readonly DependencyProperty RawCollectionProperty = DependencyProperty.Register(nameof(RawCollection), 
            typeof(IEnumerable), typeof(FilterBuilderControl));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public FilterBuilderMode Mode
        {
            get { return (FilterBuilderMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), 
            typeof(FilterBuilderMode), typeof(FilterBuilderControl), new PropertyMetadata(default(FilterBuilderMode)));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public Func<object, bool> FilteringFunc
        {
            get { return (Func<object, bool>)GetValue(FilteringFuncProperty); }
            set { SetValue(FilteringFuncProperty, value); }
        }

        public static readonly DependencyProperty FilteringFuncProperty = DependencyProperty.Register(nameof(FilteringFunc), 
            typeof(Func<object, bool>), typeof(FilterBuilderControl), new PropertyMetadata(default(Func<object, bool>)));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public IList FilteredCollection
        {
            get { return (IList)GetValue(FilteredCollectionProperty); }
            set { SetValue(FilteredCollectionProperty, value); }
        }

        public static readonly DependencyProperty FilteredCollectionProperty = DependencyProperty.Register(nameof(FilteredCollection),
            typeof(IList), typeof(FilterBuilderControl));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool AutoApplyFilter
        {
            get { return (bool)GetValue(AutoApplyFilterProperty); }
            set { SetValue(AutoApplyFilterProperty, value); }
        }

        public static readonly DependencyProperty AutoApplyFilterProperty = DependencyProperty.Register(nameof(AutoApplyFilter), 
            typeof(bool), typeof(FilterBuilderControl), new PropertyMetadata(true));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool AllowLivePreview
        {
            get { return (bool)GetValue(AllowLivePreviewProperty); }
            set { SetValue(AllowLivePreviewProperty, value); }
        }

        public static readonly DependencyProperty AllowLivePreviewProperty = DependencyProperty.Register(nameof(AllowLivePreview), 
            typeof(bool), typeof(FilterBuilderControl), new PropertyMetadata(true));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool EnableAutoCompletion
        {
            get { return (bool)GetValue(EnableAutoCompletionProperty); }
            set { SetValue(EnableAutoCompletionProperty, value); }
        }

        public static readonly DependencyProperty EnableAutoCompletionProperty = DependencyProperty.Register(nameof(EnableAutoCompletion), 
            typeof(bool), typeof(FilterBuilderControl), new PropertyMetadata(true));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool AllowReset
        {
            get { return (bool)GetValue(AllowResetProperty); }
            set { SetValue(AllowResetProperty, value); }
        }

        public static readonly DependencyProperty AllowResetProperty = DependencyProperty.Register(nameof(AllowReset), 
            typeof(bool), typeof(FilterBuilderControl), new PropertyMetadata(true));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool AllowDelete
        {
            get { return (bool)GetValue(AllowDeleteProperty); }
            set { SetValue(AllowDeleteProperty, value); }
        }

        public static readonly DependencyProperty AllowDeleteProperty = DependencyProperty.Register(nameof(AllowDelete), 
            typeof(bool), typeof(FilterBuilderControl), new PropertyMetadata(true));


        [ObsoleteEx(Message = "Will be removed", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        public Brush AccentColorBrush
        {
            get { return (Brush)GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        [ObsoleteEx(Message = "Will be removed", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register(nameof(AccentColorBrush), typeof(Brush),
            typeof(FilterBuilderControl), new PropertyMetadata(Brushes.LightGray, (sender, e) => ((FilterBuilderControl)sender).OnAccentColorBrushChanged()));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public object Scope
        {
            get { return GetValue(ScopeProperty); }
            set { SetValue(ScopeProperty, value); }
        }

        public static readonly DependencyProperty ScopeProperty = DependencyProperty.Register(nameof(Scope), typeof(object),
            typeof(FilterBuilderControl), new FrameworkPropertyMetadata((sender, e) => ((FilterBuilderControl)sender).OnScopeChanged()));
        #endregion

        #region Methods
        [ObsoleteEx(Message = "Will be removed", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        private void OnAccentColorBrushChanged()
        {
            var solidColorBrush = AccentColorBrush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                var accentColor = ((SolidColorBrush)AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary("FilterBuilder");
            }
        }

        private void OnScopeChanged()
        {
            var vm = ViewModel as FilterBuilderViewModel;
            if (vm != null)
            {
                vm.Scope = Scope;
            }
        }

        private void OnFilterPreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var filterScheme = ((FrameworkElement)sender).DataContext as FilterScheme;
            if (filterScheme != null)
            {
                var vm = ViewModel as FilterBuilderViewModel;
                if (vm != null)
                {
                    vm.SelectedFilterScheme = filterScheme;
                }
            }
        }
        #endregion
    }
}

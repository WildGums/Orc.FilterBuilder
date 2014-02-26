// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBuilderControl.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Views
{
    using System.Collections;
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
            get { return (IEnumerable) GetValue(RawCollectionProperty); }
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
        #endregion
    }
}
using System.Collections;
using System.Windows;

namespace Orc.FilterBuilder.NET40
{
    public partial class FilterBuilderControl
    {
		public static readonly DependencyProperty RawCollectionProperty = DependencyProperty.Register(
			"RawCollection", typeof(IEnumerable), typeof(FilterBuilderControl),
			new PropertyMetadata(OnRawCollectionChanged));

		public IEnumerable RawCollection
		{
			get { return (IEnumerable)GetValue(RawCollectionProperty); }
			set
			{
				SetValue(RawCollectionProperty, value);
				ViewModel.InitProperties();
			}
		}

		public static readonly DependencyProperty FilteredCollectionProperty = DependencyProperty.Register(
			"FilteredCollection", typeof(ICollection), typeof(FilterBuilderControl),
			new PropertyMetadata(OnFilteredCollectionChanged));

		public IList FilteredCollection
		{
			get { return (IList)GetValue(FilteredCollectionProperty); }
			set { SetValue(FilteredCollectionProperty, value); }
		}

	    public FilterBuilderControlModel ViewModel
	    {
		    get { return view.DataContext as FilterBuilderControlModel; }
		    set { view.DataContext = value; }
	    }

	    public FilterBuilderControl()
        {
            InitializeComponent();
			ViewModel = new FilterBuilderControlModel();
		    ViewModel.View = this;
        }

		private static void OnRawCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as FilterBuilderControl).RawCollection = (IEnumerable)e.NewValue;
		}

		private static void OnFilteredCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as FilterBuilderControl).FilteredCollection = (IList)e.NewValue;
		}
    }
}


namespace Orc.FilterBuilder
{
	/// <summary>
	/// Interaction logic for EditFilterView.xaml
	/// </summary>
	public partial class EditFilterView
	{
		public EditFilterViewModel ViewModel
		{
			get { return DataContext as EditFilterViewModel; }
			set { DataContext = value; }
		}

		public EditFilterView()
		{
			InitializeComponent();
			ViewModel = new EditFilterViewModel();
			ViewModel.View = this;
		}
	}
}

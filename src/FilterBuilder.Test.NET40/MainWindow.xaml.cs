
namespace Orc.FilterBuilder.Test.NET40
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindowViewModel ViewModel
		{
			get { return DataContext as MainWindowViewModel; }
			set { DataContext = value; }
		}

		public MainWindow()
		{
			InitializeComponent();

			ViewModel = new MainWindowViewModel();
		}
	}
}

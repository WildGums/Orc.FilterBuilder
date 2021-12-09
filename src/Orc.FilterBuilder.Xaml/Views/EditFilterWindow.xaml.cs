namespace Orc.FilterBuilder.Views
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for EditFilterWindow.xaml
    /// </summary>
    public partial class EditFilterWindow
    {
        public EditFilterWindow()
            : base(DataWindowMode.OkCancel, infoBarMessageControlGenerationMode: InfoBarMessageControlGenerationMode.None)
        {
            InitializeComponent();
        }
    }
}

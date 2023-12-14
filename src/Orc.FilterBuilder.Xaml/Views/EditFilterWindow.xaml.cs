namespace Orc.FilterBuilder.Views;

using Catel.Windows;

public partial class EditFilterWindow
{
    public EditFilterWindow()
        : base(DataWindowMode.OkCancel, infoBarMessageControlGenerationMode: InfoBarMessageControlGenerationMode.None)
    {
        InitializeComponent();
    }
}
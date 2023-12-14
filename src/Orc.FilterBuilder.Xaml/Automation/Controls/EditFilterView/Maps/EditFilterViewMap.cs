namespace Orc.FilterBuilder.Automation;

using System.Windows.Automation;
using Controls.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class EditFilterViewMap : AutomationBase
{
    public EditFilterViewMap(AutomationElement element) 
        : base(element)
    {
    }

    public Edit? FilterSchemeTitleEdit => By.Id("PART_FilterSchemeTitleTextBox").One<Edit>();
    public EditFilterConditionTree? ConditionTree => By.One<EditFilterConditionTree>();
    public CheckBox? LivePreviewCheckBox => By.Id("LivePreviewCheckBox").One<CheckBox>();
    public LinkLabel? TogglePreviewLinkLabel => By.Id("TogglePreviewLinkLabel").One<LinkLabel>();
    public DataGrid? PreviewDataGrid => By.One<DataGrid>();
}

namespace Orc.FilterBuilder.Tests;

using System.Windows.Automation;
using Orc.Automation;

public class SplitButton : AutomationControl<SplitButtonModel>
{
    public SplitButton(AutomationElement element) 
        : base(element)
    {
    }
}
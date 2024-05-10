namespace Orc.FilterBuilder.Tests;

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Orc.Automation;
using Orc.Automation.Controls;
using Orc.Automation.Tests;
using Theming;
using FrameworkElement = System.Windows.FrameworkElement;

//TODO:Vladimir: create base type in Orc.Automation
public abstract class StyledControlTestFacts<TControl> : ControlUiTestsBase<TControl>
    where TControl : FrameworkElement
{
    protected TestHostAutomationControl TestHost { get; private set; }

    protected override bool TryLoadControl(TestHostAutomationControl testHost, out string testedControlAutomationId)
    {
        var controlType = typeof(TControl);

        TestHost = testHost;

        var testDirectory = TestContext.CurrentContext.TestDirectory;

        testHost.TryLoadAssembly(Path.Combine(testDirectory, "DiffEngine.dll"));
        testHost.TryLoadAssembly(Path.Combine(testDirectory, "ApprovalUtilities.dll"));
        testHost.TryLoadAssembly(Path.Combine(testDirectory, "ApprovalTests.dll"));
        testHost.TryLoadAssembly(Path.Combine(testDirectory, "ControlzEx.dll"));
        testHost.TryLoadAssembly(Path.Combine(testDirectory, "Orc.Theming.dll"));
        testHost.TryLoadAssembly(Path.Combine(testDirectory, "Orc.Controls.dll"));
        testHost.TryLoadAssembly(Path.Combine(testDirectory, "Orc.Automation.Tests.dll"));
        testHost.TryLoadAssembly(Path.Combine(testDirectory, "Orc.FilterBuilder.dll"));
        testHost.TryLoadAssembly(Path.Combine(testDirectory, "Orc.FilterBuilder.Xaml.dll"));
        testHost.TryLoadAssembly(Path.Combine(testDirectory, "Orc.FilterBuilder.Tests.dll"));

        testHost.TryLoadResources("pack://application:,,,/Orc.Theming;component/Themes/Generic.xaml");
        testHost.TryLoadResources("pack://application:,,,/Orc.Controls;component/Themes/Generic.xaml");
        testHost.TryLoadResources("pack://application:,,,/Orc.FilterBuilder.Xaml;component/Themes/Generic.xaml");

        var result = testHost.TryLoadControlWithForwarders(controlType, out testedControlAutomationId, $"pack://application:,,,/{controlType.Assembly.GetName().Name};component/Themes/Generic.xaml");

        return result;
    }
}

public static class TestHostAutomationControlExtensions
{
    public static bool TryLoadControlWithForwarders(this TestHostAutomationControl testHost, Type controlType, out string testHostAutomationId, params string[] resources)
    {
        var controlAssembly = controlType.Assembly;

        var controlTypeFullName = controlType.FullName;
        var controlAssemblyLocation = controlAssembly.Location;

        testHostAutomationId = string.Empty;

        if (!testHost.TryLoadAssembly(controlAssemblyLocation))
        {
            testHostAutomationId = $"Error! Can't load control assembly from: {controlAssemblyLocation}";

            return false;
        }

        foreach (var resource in resources ?? Enumerable.Empty<string>())
        {
            if (!testHost.TryLoadResources(resource))
            {
                testHostAutomationId = $"Error! Can't load control resource: {resource}";
            }
        }

        //Apply style forwarders
        testHost.RunMethod(typeof(StyleHelper), nameof(StyleHelper.CreateStyleForwardersForDefaultStyles));

        testHostAutomationId = testHost.PutControl(controlTypeFullName);
        if (string.IsNullOrWhiteSpace(testHostAutomationId) || testHostAutomationId.StartsWith("Error"))
        {
            testHostAutomationId = $"Error! Can't put control inside test host control: {controlTypeFullName}";

            return false;
        }

        //Apply theme
        testHost.Execute<SynchronizeThemeAutomationMethodRun>();

        return true;
    }
}

public class SynchronizeThemeAutomationMethodRun : NamedAutomationMethodRun
{
    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
    {
        result = AutomationValue.FromValue(true);

        try
        {
            ThemeManager.Current.SynchronizeTheme();
        }
        catch (Exception e)
        {
            System.Windows.MessageBox.Show(e.Message);
        }

        return true;
    }
}

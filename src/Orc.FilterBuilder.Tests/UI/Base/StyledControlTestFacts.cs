namespace Orc.FilterBuilder.Tests
{
    using System;
    using System.Linq;
    using Orc.Automation.Controls;
    using Orc.Automation.Tests;
    using Theming;

    //TODO:Vladimir: create base type in Orc.Automation
    public abstract class StyledControlTestFacts<TControl> : ControlUiTestFactsBase<TControl>
        where TControl : System.Windows.FrameworkElement
    {
        protected override bool TryLoadControl(TestHostAutomationControl testHost, out string testedControlAutomationId)
        {
            var controlType = typeof(TControl);

            testHost.TryLoadAssembly(@"C:\Source\Orc.FilterBuilder\output\Debug\Orc.FilterBuilder.Tests\net6.0-windows\DiffEngine.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.FilterBuilder\output\Debug\Orc.FilterBuilder.Tests\net6.0-windows\ApprovalUtilities.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.FilterBuilder\output\Debug\Orc.FilterBuilder.Tests\net6.0-windows\ApprovalTests.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.FilterBuilder\output\Debug\Orc.FilterBuilder.Tests\net6.0-windows\Orc.Controls.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.FilterBuilder\output\Debug\Orc.FilterBuilder.Tests\net6.0-windows\Orc.Automation.Tests.dll");

            testHost.TryLoadAssembly(@"C:\Source\Orc.FilterBuilder\output\Debug\Orc.FilterBuilder.Tests\net6.0-windows\Orc.FilterBuilder.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.FilterBuilder\output\Debug\Orc.FilterBuilder.Tests\net6.0-windows\Orc.FilterBuilder.Xaml.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.FilterBuilder\output\Debug\Orc.FilterBuilder.Tests\net6.0-windows\Orc.FilterBuilder.Tests.dll");

            //var result = testHost.TryLoadAssembly(@"C:\Source\Orc.FilterBuilder\output\Debug\Orc.FilterBuilder.Tests\net6.0-windows\Orc.Controls.Tests.dll");

            //testHost.TryLoadResources("pack://application:,,,/Orc.FilterBuilder;component/Themes/Generic.xaml");
            testHost.TryLoadResources("pack://application:,,,/Orc.FilterBuilder.Xaml;component/Themes/Generic.xaml");

            return testHost.TryLoadControlWithForwarders(controlType, out testedControlAutomationId, $"pack://application:,,,/{controlType.Assembly.GetName().Name};component/Themes/Generic.xaml");
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
            //testHost.ExecuteAutomationMethod<CreateStyleForwardersMethodRun>();

            testHostAutomationId = testHost.PutControl(controlTypeFullName);
            if (string.IsNullOrWhiteSpace(testHostAutomationId) || testHostAutomationId.StartsWith("Error"))
            {
                testHostAutomationId = $"Error! Can't put control inside test host control: {controlTypeFullName}";

                return false;
            }

            return true;
        }
    }
}

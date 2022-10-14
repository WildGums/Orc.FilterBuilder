namespace Orc.FilterBuilder.Tests
{
    using System.Windows;
    using Catel.IoC;
    using Orc.Automation;
    using ViewModels;
    using Views;

    public class InitViewModelMethodRun : NamedAutomationMethodRun
    {
        public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            result = AutomationValue.FromValue(true);

            if (owner is not FilterBuilderControl control)
            {
                return true;
            }

            foreach (var scope in FilterBuilderControlTestData.AvailableScopes)
            {
                RegisterScope(scope);
            }

#pragma warning disable IDISP004 // Don't ignore created IDisposable
            var vm = this.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<FilterBuilderViewModel>();
#pragma warning restore IDISP004 // Don't ignore created IDisposable

            control.DataContext = vm;

            return true;
        }

        private void RegisterScope(object scope)
        {
#pragma warning disable IDISP001 // Dispose created
            var serviceLocator = this.GetServiceLocator();
            var typeFactory = this.GetTypeFactory();
#pragma warning restore IDISP001 // Dispose created
            var filterManager = new TestFilterSchemeManager
            {
                Scope = scope
            };

            var reflectionService = typeFactory.CreateInstanceWithParametersAndAutoCompletion<ReflectionService>();
            var filterService = typeFactory.CreateInstanceWithParametersAndAutoCompletion<FilterService>();

            serviceLocator.RegisterInstance(typeof(IReflectionService), reflectionService, scope);
            serviceLocator.RegisterInstance(typeof(IFilterService), filterService, scope);
            serviceLocator.RegisterInstance(typeof(IFilterSchemeManager), filterManager, scope);
        }
    }
}

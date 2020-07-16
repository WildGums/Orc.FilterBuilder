using Catel.IoC;
using FilterBuilder.Example.Services;
using Orc.FilterBuilder;
using Orc.FilterBuilder.Example.Services;
using Orchestra.Services;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<IRibbonService, RibbonService>();
        serviceLocator.RegisterType<IApplicationInitializationService, ApplicationInitializationService>();
        serviceLocator.RegisterType<ITestDataService, TestDataService>();
        serviceLocator.RegisterType<IFilterSerializationService, ExampleFilterSerializationService>();
    }
}

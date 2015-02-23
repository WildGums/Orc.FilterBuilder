using Catel.IoC;
using Catel.MVVM.Views;
using Orc.FilterBuilder.Services;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    private static bool _initialized;

    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterTypeIfNotYetRegistered<IFilterService, FilterService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IReflectionService, ReflectionService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IFilterSchemeManager, FilterSchemeManager>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IPreviewGeneratorService, TypedPreviewGeneratorService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IFilterCustomizationService, FilterCustomizationService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IAutoCompletionService, AutoCompletionService>();
    }
}
using Catel.IoC;
using Catel.Services;
using Orc.FilterBuilder;
using Orc.FilterBuilder.Services;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    #region Methods
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterTypeIfNotYetRegistered<IFilterService, FilterService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IReflectionService, ReflectionService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IFilterSchemeManager, FilterSchemeManager>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IFilterCustomizationService, FilterCustomizationService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IFilterSerializationService, FilterSerializationService>();

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.FilterBuilder", "Orc.FilterBuilder.Properties", "Resources"));
    }
    #endregion
}

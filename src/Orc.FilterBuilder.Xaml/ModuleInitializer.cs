using Catel.IoC;
using Catel.Services;
using Orc.FilterBuilder.ViewModels;
using Orc.FilterBuilder.Views;

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

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.FilterBuilder.Xaml", "Orc.FilterBuilder.Properties", "Resources"));

        // Register some custom windows (since we combine windows and views)
        var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
        uiVisualizerService.Register<EditFilterViewModel, EditFilterWindow>();
    }
    #endregion
}

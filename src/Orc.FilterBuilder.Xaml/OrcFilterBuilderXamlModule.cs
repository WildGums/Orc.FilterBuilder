namespace Orc.FilterBuilder
{
    using Catel.IoC;
    using Catel.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Orc.FilterBuilder.ViewModels;
    using Orc.FilterBuilder.Views;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrcFilterBuilderXamlModule
    {
        public static IServiceCollection AddOrcFilterBuilderXaml(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<UIVisualizerInitializer>();

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.FilterBuilder.Xaml", "Orc.FilterBuilder.Properties", "Resources"));

            return serviceCollection;
        }

        private class UIVisualizerInitializer : IConstructAtStartup
        {
            public UIVisualizerInitializer(IUIVisualizerService uiVisualizerService)
            {
                uiVisualizerService.Register<EditFilterViewModel, EditFilterWindow>();
            }
        }
    }
}

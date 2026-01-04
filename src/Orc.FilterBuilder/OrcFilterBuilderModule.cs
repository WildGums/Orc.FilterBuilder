namespace Orc.FilterBuilder
{
    using Catel.Services;
    using Catel.ThirdPartyNotices;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrcFilterBuilderModule
    {
        public static IServiceCollection AddOrcFilterBuilder(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IFilterService, FilterService>();
            serviceCollection.TryAddSingleton<IReflectionService, ReflectionService>();
            serviceCollection.TryAddSingleton<IFilterSchemeManager, FilterSchemeManager>();
            serviceCollection.TryAddSingleton<IFilterCustomizationService, FilterCustomizationService>();
            serviceCollection.TryAddSingleton<IFilterSerializationService, FilterSerializationService>();

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.FilterBuilder", "Orc.FilterBuilder.Properties", "Resources"));

            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new LibraryThirdPartyNotice("Orc.FilterBuilder", "https://github.com/wildgums/orc.filterbuilder"));

            return serviceCollection;
        }
    }
}

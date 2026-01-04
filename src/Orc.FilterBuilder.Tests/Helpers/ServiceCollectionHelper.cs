namespace Orc.FilterBuilder.Tests
{
    using Catel;
    using Microsoft.Extensions.DependencyInjection;
    using Orc.FileSystem;
    using Orc.Serialization.Json;

    internal static class ServiceCollectionHelper
    {
        public static IServiceCollection CreateServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging();
            serviceCollection.AddCatelCore();
            serviceCollection.AddOrcFileSystem();
            serviceCollection.AddOrcFilterBuilder();
            serviceCollection.AddOrcFilterBuilderXaml();
            serviceCollection.AddOrcSerializationJson();

            return serviceCollection;
        }
    }
}

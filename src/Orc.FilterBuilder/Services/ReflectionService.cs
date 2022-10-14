namespace Orc.FilterBuilder
{
    using System;
    using Catel.Caching;

    public class ReflectionService : IReflectionService
    {
        private readonly IFilterCustomizationService _filterCustomizationService;

        private readonly ICacheStorage<Type, IPropertyCollection> _cache = new CacheStorage<Type, IPropertyCollection>();
        
        public ReflectionService(IFilterCustomizationService filterCustomizationService)
        {
            ArgumentNullException.ThrowIfNull(filterCustomizationService);

            _filterCustomizationService = filterCustomizationService;
        }

        public IPropertyCollection GetInstanceProperties(Type targetType)
        {
            ArgumentNullException.ThrowIfNull(targetType);

            return _cache.GetFromCacheOrFetch(targetType, () =>
            {
                var instanceProperties = new InstanceProperties(targetType);
                
                _filterCustomizationService.CustomizeInstanceProperties(instanceProperties);

                return instanceProperties;
            });
        }

        public void ClearCache()
        {
            _cache.Clear();
        }
    }
}

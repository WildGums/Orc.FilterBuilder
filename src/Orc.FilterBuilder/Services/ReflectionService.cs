// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Caching;
    using Catel.Threading;

    public class ReflectionService : IReflectionService
    {
        private readonly IFilterCustomizationService _filterCustomizationService;

        #region Fields
        private readonly ICacheStorage<Type, IPropertyCollection> _cache = new CacheStorage<Type, IPropertyCollection>();
        #endregion

        public ReflectionService(IFilterCustomizationService filterCustomizationService)
        {
            Argument.IsNotNull(() => filterCustomizationService);

            _filterCustomizationService = filterCustomizationService;
        }

        #region Methods
        public IPropertyCollection GetInstanceProperties(Type targetType)
        {
            Argument.IsNotNull(() => targetType);

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
        #endregion
    }
}

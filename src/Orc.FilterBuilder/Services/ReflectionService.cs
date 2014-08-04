// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System;
    using Catel;
    using Catel.Caching;
    using Orc.FilterBuilder.Models;

    public class ReflectionService : IReflectionService
    {
        private readonly IFilterCustomizationService _filterCustomizationService;

        #region Fields
        private readonly ICacheStorage<Type, InstanceProperties> _cache = new CacheStorage<Type, InstanceProperties>();
        #endregion

        public ReflectionService(IFilterCustomizationService filterCustomizationService)
        {
            Argument.IsNotNull(() => filterCustomizationService);

            _filterCustomizationService = filterCustomizationService;
        }

        #region Methods
        public InstanceProperties GetInstanceProperties(Type targetType)
        {
            Argument.IsNotNull("targetType", targetType);

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
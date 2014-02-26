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
        #region Fields
        private readonly ICacheStorage<Type, InstanceProperties> _cache = new CacheStorage<Type, InstanceProperties>();
        #endregion

        #region Methods
        public InstanceProperties GetInstanceProperties(Type targetType)
        {
            Argument.IsNotNull("targetType", targetType);

            return _cache.GetFromCacheOrFetch(targetType, () => new InstanceProperties(targetType));
        }
        #endregion
    }
}
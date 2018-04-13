// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReflectionService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System;
    using Models;

    public interface IReflectionService
    {
        IPropertyCollection GetInstanceProperties(Type targetType);

        void ClearCache();
    }
}
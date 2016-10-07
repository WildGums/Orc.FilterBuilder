// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReflectionService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System;
    using System.Threading.Tasks;
    using Models;

    public interface IReflectionService
    {
        IPropertyCollection GetInstanceProperties(Type targetType);

        [ObsoleteEx(ReplacementTypeOrMember = "GetInstanceProperties", TreatAsErrorFromVersion = "1.0", RemoveInVersion = "2.0")]
        Task<IPropertyCollection> GetInstancePropertiesAsync(Type targetType);

        void ClearCache();
    }
}
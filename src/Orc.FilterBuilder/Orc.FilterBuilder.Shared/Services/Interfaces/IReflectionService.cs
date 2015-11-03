// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReflectionService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System;
    using System.Threading.Tasks;
    using Orc.FilterBuilder.Models;

    public interface IReflectionService
    {
        [ObsoleteEx(ReplacementTypeOrMember = "GetInstancePropertiesAsync", TreatAsErrorFromVersion = "1.0", RemoveInVersion = "2.0")]
        IPropertyCollection GetInstanceProperties(Type targetType);

        Task<IPropertyCollection> GetInstancePropertiesAsync(Type targetType);

        void ClearCache();
    }
}
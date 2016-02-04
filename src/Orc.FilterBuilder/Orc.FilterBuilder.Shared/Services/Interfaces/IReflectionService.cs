// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReflectionService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
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
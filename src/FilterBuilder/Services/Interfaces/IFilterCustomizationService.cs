// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilterCustomizationService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using Orc.FilterBuilder.Models;

    public interface IFilterCustomizationService
    {
        void CustomizeInstanceProperties(InstanceProperties instanceProperties);
    }
}
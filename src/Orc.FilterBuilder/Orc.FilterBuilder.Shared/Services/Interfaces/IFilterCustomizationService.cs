// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilterCustomizationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using Orc.FilterBuilder.Models;

    public interface IFilterCustomizationService
    {
        void CustomizeInstanceProperties(IPropertyCollection instanceProperties);
    }
}
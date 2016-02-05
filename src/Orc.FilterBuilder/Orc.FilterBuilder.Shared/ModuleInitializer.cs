// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using Catel.IoC;
using Catel.Services;
using Catel.Services.Models;
using Orc.FilterBuilder.Properties;
using Orc.FilterBuilder.Services;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    #region Methods
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterTypeIfNotYetRegistered<IFilterService, FilterService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IReflectionService, ReflectionService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IFilterSchemeManager, FilterSchemeManager>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IFilterCustomizationService, FilterCustomizationService>();

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.FilterBuilder", "Orc.FilterBuilder.Properties", "Resources"));
    }
    #endregion
}
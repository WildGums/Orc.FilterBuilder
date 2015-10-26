// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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

        RegisterLanguageSources(serviceLocator);
    }

    private static void RegisterLanguageSources(IServiceLocator serviceLocator)
    {
        var languageService = serviceLocator.TryResolveType<ILanguageService>();
        var stringsResourceType = typeof(Strings);
        languageService.RegisterLanguageSource(new LanguageResourceSource(stringsResourceType.Assembly.FullName, stringsResourceType.Namespace, stringsResourceType.Name));
    }
    #endregion
}
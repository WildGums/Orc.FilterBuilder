// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Example
{
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using Catel.ApiCop;
    using Catel.ApiCop.Listeners;
    using Catel.Logging;
    using Orc.FilterBuilder.Services;
    using Catel.IoC;
    using Catel.Services;
    using Catel.Windows;
    using Orchestra.Services;
    using Orchestra.Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            //LogManager.AddDebugListener();
#endif

            var languageService = ServiceLocator.Default.ResolveType<ILanguageService>();

            // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
            // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
            // we use .CurrentCulture for the sake of the demo
            languageService.PreferredCulture = CultureInfo.CurrentCulture;
            languageService.FallbackCulture = new CultureInfo("en-US");

            //Log.Info("Starting application");

            StyleHelper.CreateStyleForwardersForDefaultStyles();

            //Log.Info("Calling base.OnStartup");

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveType<IShellService>();
            await shellService.CreateWithSplashAsync<ShellWindow>();

            var filterSchemeManager = serviceLocator.ResolveType<IFilterSchemeManager>();
            filterSchemeManager.Load();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Get advisory report in console
            ApiCopManager.AddListener(new ConsoleApiCopListener());
            ApiCopManager.WriteResults();

            base.OnExit(e);
        }
    }
}
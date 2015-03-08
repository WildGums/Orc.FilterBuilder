namespace Orc.FilterBuilder.AlternativeExample
{
    using System.Windows;
    using Catel.ApiCop;
    using Catel.ApiCop.Listeners;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Windows;
    using FilterBuilder.Services;
    using Orchestra.Services;
    using Orchestra.Views;
    using Services;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            LogManager.AddDebugListener();
#endif

            //Log.Info("Starting application");

            StyleHelper.CreateStyleForwardersForDefaultStyles();

            //Log.Info("Calling base.OnStartup");

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveType<IShellService>();
            await shellService.CreateWithSplash<ShellWindow>();

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

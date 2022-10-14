namespace Orc.FilterBuilder.Example
{
    using System.Globalization;
    using System.Windows;
    using Catel.Logging;
    using Catel.IoC;
    using Catel.Services;
    using Orchestra;
    using Orchestra.Services;
    using Orchestra.Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
#pragma warning disable AvoidAsyncVoid
        protected override async void OnStartup(StartupEventArgs e)
#pragma warning restore AvoidAsyncVoid
        {
#if DEBUG
            LogManager.AddDebugListener();
#endif

            var languageService = ServiceLocator.Default.ResolveRequiredType<ILanguageService>();

            // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
            // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
            // we use .CurrentCulture for the sake of the demo
            languageService.PreferredCulture = CultureInfo.CurrentCulture;
            languageService.FallbackCulture = new CultureInfo("en-US");

            //Log.Info("Starting application");

            this.ApplyTheme();

            //Log.Info("Calling base.OnStartup");

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveRequiredType<IShellService>();
            await shellService.CreateAsync<ShellWindow>();

            var filterSchemeManager = serviceLocator.ResolveRequiredType<IFilterSchemeManager>();
            await filterSchemeManager.LoadAsync();

            base.OnStartup(e);
        }
    }
}

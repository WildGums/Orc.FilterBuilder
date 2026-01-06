namespace Orc.FilterBuilder.Example;

using System.Globalization;
using System.Windows;
using Catel;
using Catel.IoC;
using Catel.Services;
using global::FilterBuilder.Example.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orc.Automation;
using Orc.Controls;
using Orc.FileSystem;
using Orc.FilterBuilder.Example.Services;
using Orc.Serialization.Json;
using Orc.SystemInfo;
using Orc.Theming;
using Orchestra;
using Orchestra.Views;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

#pragma warning disable IDISP006 // Implement IDisposable
    private readonly IHost _host;
#pragma warning restore IDISP006 // Implement IDisposable

    public App()
    {
        var hostBuilder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddCatelCore();
                services.AddCatelMvvm();
                services.AddOrcAutomation();
                services.AddOrcControls();
                services.AddOrcFilterBuilder();
                services.AddOrcFilterBuilderXaml();
                services.AddOrcFileSystem();
                services.AddOrcSerializationJson();
                services.AddOrcSystemInfo();
                services.AddOrcTheming();
                services.AddOrchestraCore();
                services.AddOrchestraShellRibbonFluent();

                services.AddSingleton<IRibbonService, RibbonService>();
                services.AddSingleton<IApplicationInitializationService, ApplicationInitializationService>();

                services.AddSingleton<ITestDataService, TestDataService>();
                services.AddSingleton<IFilterSerializationService, ExampleFilterSerializationService>();

                services.AddLogging(x =>
                {
                    x.AddConsole();
                    x.AddDebug();
                });
            });

        _host = hostBuilder.Build();

        IoCContainer.ServiceProvider = _host.Services;
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceProvider = IoCContainer.ServiceProvider;

        serviceProvider.CreateTypesThatMustBeConstructedAtStartup();

        var languageService = serviceProvider.GetRequiredService<ILanguageService>();

        // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
        // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
        // we use .CurrentCulture for the sake of the demo
        languageService.PreferredCulture = CultureInfo.CurrentCulture;
        languageService.FallbackCulture = new CultureInfo("en-US");

        this.ApplyTheme();

        var shellService = serviceProvider.GetRequiredService<IShellService>();
        await shellService.CreateAsync<ShellWindow>();

        var filterSchemeManager = serviceProvider.GetRequiredService<IFilterSchemeManager>();
        await filterSchemeManager.LoadAsync();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }

        base.OnExit(e);
    }
}

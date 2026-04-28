namespace Orc.FilterBuilder.Example.Services;

using System;
using System.Threading.Tasks;
using System.Windows.Media;
using Orchestra;

public class ApplicationInitializationService : ApplicationInitializationServiceBase
{
    public ApplicationInitializationService(IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
    }

    public override Task InitializeBeforeCreatingShellAsync()
    {
        InitializeFonts();

        return Task.CompletedTask;
    }

    private void InitializeFonts()
    {
        Theming.FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orc.FilterBuilder.Example;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));
        Theming.FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
        Theming.FontImage.DefaultFontFamily = "FontAwesome";
    }
}

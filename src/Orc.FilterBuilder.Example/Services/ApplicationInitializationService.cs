namespace Orc.FilterBuilder.Example.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel;
    using Catel.IoC;
    using Catel.Threading;
    using Orc.Controls;
    using Orchestra.Services;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private readonly IServiceLocator _serviceLocator;

        public ApplicationInitializationService(IServiceLocator serviceLocator)
        {
            ArgumentNullException.ThrowIfNull(serviceLocator);

            _serviceLocator = serviceLocator;
        }

        public override Task InitializeBeforeCreatingShellAsync()
        {
            InitializeFonts();

            return Task.CompletedTask;
        }

        private void InitializeFonts()
        {
            Orc.Theming.FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orc.FilterBuilder.Example;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));
            Orc.Theming.FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
            Orc.Theming.FontImage.DefaultFontFamily = "FontAwesome";
        }
    }
}

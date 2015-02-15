using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.FilterBuilder.AlternativeExample.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel;
    using Catel.IoC;
    using Orchestra.Markup;
    using Orchestra.Services;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private readonly IServiceLocator _serviceLocator;

        public ApplicationInitializationService(IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => serviceLocator);

            _serviceLocator = serviceLocator;
        }

        public override async Task InitializeBeforeCreatingShell()
        {
            await InitializeFonts();
        }

        private async Task InitializeFonts()
        {
            FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orc.FilterBuilder.AlternativeExample;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));

            FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 187, 187, 87));
            FontImage.DefaultFontFamily = "FontAwesome";
        }
    }
}

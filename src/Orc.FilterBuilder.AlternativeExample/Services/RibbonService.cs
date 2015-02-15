namespace Orc.FilterBuilder.AlternativeExample.Services
{
    using System.Windows;
    using Orchestra.Shell.Services;
    using Views;

    public class RibbonService : IRibbonService
    {
        #region IRibbonService Members
        public FrameworkElement GetRibbon()
        {
            return new RibbonView();
        }

        public FrameworkElement GetMainView()
        {
            return new MainView();
        }

        public FrameworkElement GetStatusBar()
        {
            return new StatusBarView();
        }
        #endregion
    }
}

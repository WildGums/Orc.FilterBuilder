// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Test
{
    using System.Windows;
    using Catel.Logging;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            //LogManager.AddDebugListener();
#endif

            base.OnStartup(e);
        }
    }
}
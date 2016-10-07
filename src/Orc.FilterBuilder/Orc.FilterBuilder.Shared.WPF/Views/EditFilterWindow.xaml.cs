// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditFilterWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Views
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for EditFilterWindow.xaml
    /// </summary>
    public partial class EditFilterWindow
    {
        public EditFilterWindow()
            : base(DataWindowMode.OkCancel, infoBarMessageControlGenerationMode: InfoBarMessageControlGenerationMode.None)
        {
            InitializeComponent();
        }
    }
}
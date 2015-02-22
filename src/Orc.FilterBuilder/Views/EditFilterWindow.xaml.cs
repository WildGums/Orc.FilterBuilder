// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditFilterView.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Views
{
    using System.Windows.Controls;
    using System.Windows.Data;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Reflection;
    using Catel.Windows;
    using Converters;
    using Services;
    using ViewModels;

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



        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            previewPlaceholder.Content = null;

            var vm = ViewModel as EditFilterViewModel;
            if (vm != null)
            {
                if (vm.AllowLivePreview)
                {
                    var dependencyResolver = this.GetDependencyResolver();
                    var previewGeneratorService = dependencyResolver.Resolve<IPreviewGeneratorService>();
                    previewPlaceholder.Content = previewGeneratorService.GeneratePreviewControl(vm.FilterScheme.TargetDataDescriptor, vm, "PreviewItems");
                }
            }
        }
    }
}
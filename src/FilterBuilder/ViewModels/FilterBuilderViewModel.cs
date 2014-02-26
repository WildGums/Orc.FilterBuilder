// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBuilderControlModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.ViewModels
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.FilterBuilder.Models;
    using Orc.FilterBuilder.Services;

    public class FilterBuilderViewModel : ViewModelBase
    {
        private readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IFilterSchemeManager _filterSchemeManager;

        #region Constructors
        public FilterBuilderViewModel(IUIVisualizerService uiVisualizerService, IFilterSchemeManager filterSchemeManager)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => filterSchemeManager);

            _uiVisualizerService = uiVisualizerService;
            _filterSchemeManager = filterSchemeManager;

            FilterSchemes = _filterSchemeManager.FilterSchemes;

            NewSchemeCommand = new Command(OnNewScheme);
            EditSchemeCommand = new Command(OnEditScheme, () => SelectedFilterScheme != null);
            ApplySchemeCommand = new Command(OnApplyScheme, () => SelectedFilterScheme != null);
        }
        #endregion

        #region Properties
        public FilterSchemes FilterSchemes { get; private set; }
        public FilterScheme SelectedFilterScheme { get; set; }

        public IEnumerable RawCollection { get; set; }
        public IList FilteredCollection { get; set; }

        public Command NewSchemeCommand { get; private set; }
        public Command EditSchemeCommand { get; private set; }
        public Command ApplySchemeCommand { get; private set; }
        #endregion

        #region Commands
        #endregion

        #region Methods
        public void OnNewScheme()
        {
            if (RawCollection == null)
            {
                Log.Warning("RawCollection is null, cannot get any type information to create filters");
                return;
            }

            var enumerator = RawCollection.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                Log.Warning("RawCollection does not contain items, cannot get any type information to create filters");
                return;
            }

            var firstElement = enumerator.Current;
            var filterScheme = new FilterScheme(firstElement.GetType());

            if (_uiVisualizerService.ShowDialog<EditFilterViewModel>(filterScheme) ?? false)
            {
                FilterSchemes.Schemes.Add(filterScheme);
                SelectedFilterScheme = filterScheme;

                _filterSchemeManager.Save();
            }
        }

        public void OnEditScheme()
        {
            if (_uiVisualizerService.ShowDialog<EditFilterViewModel>(SelectedFilterScheme) ?? false)
            {
                _filterSchemeManager.Save();
            }
        }

        public void OnApplyScheme()
        {
            FilteredCollection.Clear();
            foreach (object item in RawCollection)
            {
                if (SelectedFilterScheme.CalculateResult(item))
                {
                    FilteredCollection.Add(item);
                }
            }
        }

        protected override void Initialize()
        {
            SelectedFilterScheme = FilterSchemes.Schemes.FirstOrDefault();
        }
        #endregion
    }
}
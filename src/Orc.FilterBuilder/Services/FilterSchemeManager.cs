// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeManager.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Threading;
    using Models;

    public class FilterSchemeManager : IFilterSchemeManager
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly string DefaultFileName = Path.Combine(Catel.IO.Path.GetApplicationDataDirectory(), "FilterSchemes.xml");
        #endregion

        #region Fields
        private readonly IFilterSerializationService _filterSerializationService;

        private string _lastFileName;
        private object _scope;
        #endregion

        #region Constructors
        public FilterSchemeManager(IFilterSerializationService filterSerializationService)
        {
            Argument.IsNotNull(() => filterSerializationService);

            _filterSerializationService = filterSerializationService;

            AutoSave = true;
            FilterSchemes = new FilterSchemes();
        }
        #endregion

        #region Properties
        public bool AutoSave { get; set; }

        public FilterSchemes FilterSchemes { get; private set; }

        public object Scope
        {
            get { return _scope; }
            set
            {
                _scope = value;
                FilterSchemes.Scope = _scope;
            }
        }
        #endregion

        #region IFilterSchemeManager Members
        public event EventHandler<EventArgs> Updated;

        public event EventHandler<EventArgs> Loaded;

        public event EventHandler<EventArgs> Saved;

        [ObsoleteEx(ReplacementTypeOrMember = "UpdateFiltersAsync", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        public async void UpdateFilters()
        {
            await UpdateFiltersAsync();
        }

        [ObsoleteEx(ReplacementTypeOrMember = "IFilterSerializationService.LoadFiltersAsync", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        public async void Load(string fileName = null)
        {
            await LoadAsync();
        }

        [ObsoleteEx(ReplacementTypeOrMember = "IFilterSerializationService.SaveFiltersAsync", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        public async void Save(string fileName = null)
        {
            await SaveAsync();
        }

        public virtual async Task UpdateFiltersAsync()
        {
            try
            {
                Updated?.Invoke(this, EventArgs.Empty);

                if (AutoSave)
                {
                    await SaveAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to update filters");
                throw;
            }
        }

        public virtual async Task<bool> LoadAsync(string fileName = null)
        {
            fileName = GetFileName(fileName);

            var filterSchemes = await _filterSerializationService.LoadFiltersAsync(fileName);
            filterSchemes.Scope = Scope;

            FilterSchemes = filterSchemes;

            Loaded?.Invoke(this, EventArgs.Empty);
            Updated?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public virtual async Task SaveAsync(string fileName = null)
        {
            fileName = GetFileName(fileName);

            await _filterSerializationService.SaveFiltersAsync(fileName, FilterSchemes);

            Saved?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Methods
        private string GetFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = _lastFileName ?? DefaultFileName;
            }

            _lastFileName = fileName;

            return fileName;
        }
        #endregion
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeManager.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Catel.Threading;

    public class FilterSchemeManager : IFilterSchemeManager
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly IFilterSerializationService _filterSerializationService;
        private readonly IAppDataService _appDataService;
        private string _lastFileName;
        private object _scope;
        #endregion

        #region Constructors
        public FilterSchemeManager(IFilterSerializationService filterSerializationService, IAppDataService appDataService)
        {
            Argument.IsNotNull(() => filterSerializationService);
            Argument.IsNotNull(() => appDataService);

            _filterSerializationService = filterSerializationService;
            _appDataService = appDataService;

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

            _lastFileName = fileName;

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
        protected virtual string GetDefaultFileName()
        {
            var defaultFileName = Path.Combine(_appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "FilterSchemes.xml");
            return defaultFileName;
        }

        private string GetFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = _lastFileName ?? GetDefaultFileName();
            }

            _lastFileName = fileName;

            return fileName;
        }
        #endregion
    }
}

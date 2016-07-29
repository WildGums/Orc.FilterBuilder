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
    using Catel.Runtime.Serialization.Xml;
    using Catel.Threading;
    using Models;

    public class FilterSchemeManager : IFilterSchemeManager
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private static readonly string DefaultFileName = Path.Combine(Catel.IO.Path.GetApplicationDataDirectory(), "FilterSchemes.xml");
        #endregion

        #region Fields
        private readonly IXmlSerializer _xmlSerializer;
        private string _lastFileName;
        private readonly AsyncLock _lockObject = new AsyncLock();
        private object _scope;
        #endregion

        #region Constructors
        public FilterSchemeManager(IXmlSerializer xmlSerializer)
        {
            Argument.IsNotNull(() => xmlSerializer);

            _xmlSerializer = xmlSerializer;

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

        public void UpdateFilters()
        {
            try
            {
                Updated.SafeInvoke(this);

                if (AutoSave)
                {
                    Save();
                }
            }
            catch (Exception ex)
            {
                Log.Error	();
                throw;
            }            
        }
        
        public void Load(string fileName = null)
        {
            if (!TryLoad(fileName))
            {
                throw Log.ErrorAndCreateException<FileLoadException>("Unable to load filters from file '{0}'", GetFileName(fileName));
            }

            Loaded.SafeInvoke(this);
            Updated.SafeInvoke(this);
        }

        public async Task<bool> LoadAsync(string fileName = null)
        {
            using (await _lockObject.LockAsync())
            {
                if (TryLoad(fileName))
                {
                    Loaded.SafeInvoke(this);
                    Updated.SafeInvoke(this);

                    return true;
                }
            }

            return false;
        }

        public void Save(string fileName = null)
        {
            fileName = GetFileName(fileName);

            Log.Info("Saving filter schemes to '{0}'", fileName);

            try
            {
                using (var stream = File.Open(fileName, FileMode.Create))
                {
                    _xmlSerializer.Serialize(FilterSchemes, stream);
                }

                Saved.SafeInvoke(this);

                Log.Debug("Saved filter schemes to '{0}'", fileName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save filter schemes");
            }
        }
        #endregion

        #region Methods
        private bool TryLoad(string fileName = null)
        {
            fileName = GetFileName(fileName);

            Log.Info("Loading filter schemes from '{0}'", fileName);

            FilterSchemes = new FilterSchemes();

            try
            {
                if (File.Exists(fileName))
                {
                    using (var stream = File.Open(fileName, FileMode.Open))
                    {
                        _xmlSerializer.Deserialize(FilterSchemes, stream);
                    }
                }

                Log.Debug("Loaded filter schemes from '{0}'", fileName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load filter schemes");
                return false;
            }

            FilterSchemes.Scope = Scope;
            return true;
        }

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
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeManager.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System;
    using System.IO;
    using Catel;
    using Catel.Logging;
    using Catel.Runtime.Serialization.Xml;
    using Orc.FilterBuilder.Models;

    public class FilterSchemeManager : IFilterSchemeManager
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Constants
        private static readonly string DefaultFileName = Path.Combine(Catel.IO.Path.GetApplicationDataDirectory(), "FilterSchemes.xml");
        #endregion

        private readonly IXmlSerializer _xmlSerializer;
        private string _lastFileName;

        public FilterSchemeManager(IXmlSerializer xmlSerializer)
        {
            Argument.IsNotNull(() => xmlSerializer);

            _xmlSerializer = xmlSerializer;

            AutoSave = true;
            FilterSchemes = new FilterSchemes();
        }

        public bool AutoSave { get; set; }

        public FilterSchemes FilterSchemes { get; private set; }

        public event EventHandler<EventArgs> Updated;

        public event EventHandler<EventArgs> Loaded;

        public event EventHandler<EventArgs> Saved;

        public void UpdateFilters()
        {
            Updated.SafeInvoke(this);

            if (AutoSave)
            {
                Save();
            }
        }

        public void Load(string fileName = null)
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
            }

            Loaded.SafeInvoke(this);
            Updated.SafeInvoke(this);
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

        private string GetFileName(string fileName)
        {
            if (fileName == null)
            {
                fileName = _lastFileName;

                if (fileName == null)
                {
                    fileName = DefaultFileName;
                }
            }

            _lastFileName = fileName;

            return fileName;
        }
    }
}
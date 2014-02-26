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
        private readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Constants
        private const string FileName = "FilterSchemes.xml";
        #endregion

        private readonly IXmlSerializer _xmlSerializer;
        private readonly string _fileName;

        public FilterSchemeManager(IXmlSerializer xmlSerializer)
        {
            Argument.IsNotNull(() => xmlSerializer);

            _xmlSerializer = xmlSerializer;
            _fileName = Path.Combine(Catel.IO.Path.GetApplicationDataDirectory(), FileName);

            FilterSchemes = new FilterSchemes();
        }

        public FilterSchemes FilterSchemes { get; private set; }

        public void Save()
        {
            Log.Info("Saving filter schemes to '{0}'", _fileName);

            try
            {
                using (var stream = File.Open(_fileName, FileMode.Create))
                {
                    _xmlSerializer.Serialize(FilterSchemes, stream);
                }

                Log.Debug("Saved filter schemes to '{0}'", _fileName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save filter schemes");
            }
        }

        public void Load()
        {
            Log.Info("Loading filter schemes from '{0}'", _fileName);

            try
            {
                if (File.Exists(_fileName))
                {
                    using (var stream = File.Open(_fileName, FileMode.Open))
                    {
                        _xmlSerializer.Deserialize(FilterSchemes, stream);
                    }

                    Log.Info("Loaded filter schemes from '{0}'", _fileName);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load filter schemes");
            }
        }
    }
}
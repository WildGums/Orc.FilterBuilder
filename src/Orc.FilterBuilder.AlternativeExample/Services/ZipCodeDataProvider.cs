﻿namespace Orc.FilterBuilder.AlternativeExample.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mime;
    using Models;
    using Newtonsoft.Json;
    using Path = Catel.IO.Path;

    public class ZipCodeDataProvider: IDataProvider
    {
        private IEnumerable<Dictionary<string, object>> _zips;
 
        public IEnumerable<object> GetData()
        {
            AssertZipsContent();
            return _zips;
        }

        public IMetadataProvider GetMetadata()
        {
            var schema = new Dictionary<string, Type>
            {
                {"city", typeof(string)},
                {"loc", typeof(Array)},
                {"pop", typeof(int)},
                {"state", typeof(string)},
                {"_id", typeof(string)}
            };

            return new DictionaryMetadataProvider(schema);
        }

        private void AssertZipsContent()
        {
            if (_zips != null)
            {
                return;
            }

            // Use "zips.json" in order to see how it works with larger datasets
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "small_zips.json");
            string[] lines = File.ReadAllLines(path);

            var collection = new List<Dictionary<string, object>>();
            foreach (var line in lines)
            {
                var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(line);
                collection.Add(obj);
            }

            _zips = collection;
        }
    }
}

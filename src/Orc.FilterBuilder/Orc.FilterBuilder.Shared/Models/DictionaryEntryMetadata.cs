// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryEntryMetadata.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Catel.Reflection;

    public class DictionaryEntryMetadata : IPropertyMetadata
    {
        private Type _expectedType;
        private string _key;

        public DictionaryEntryMetadata()
        {
        }

        public DictionaryEntryMetadata(string key, Type expectedType)
            : this()
        {
            Argument.IsNotNull(() => key);
            Argument.IsNotNull(() => expectedType);

            _key = key;
            DisplayName = key;
            _expectedType = expectedType;
        }

        public string DisplayName { get; set; }

        public string Name
        {
            get { return _key; }
        }

        public Type Type
        {
            get { return _expectedType; }
        }

        public object GetValue(object instance)
        {
            Argument.IsOfType(() => instance, typeof (IDictionary<string, object>));

            object result = null;

            var dictionary = instance as IDictionary<string, object>;
            if (dictionary != null)
            {
                dictionary.TryGetValue(_key, out result);
            }

            return result;
        }

        public TValue GetValue<TValue>(object instance)
        {
            return (TValue) GetValue(instance);
        }

        public void SetValue(object instance, object value)
        {
            Argument.IsOfType(() => instance, typeof (IDictionary<string, object>));

            var dictionary = instance as IDictionary<string, object>;
            if (dictionary == null)
            {
                return;
            }

            dictionary[_key] = value;
        }

        public string SerializeState()
        {
            return string.Format("{0}:{1}", _key, _expectedType.FullName);
        }

        public void DeserializeState(string contentAsString)
        {
            var kvp = contentAsString.Split(':');
            _key = kvp[0];
            _expectedType = TypeCache.GetType(kvp[1]);
            DisplayName = _key;
        }
    }
}
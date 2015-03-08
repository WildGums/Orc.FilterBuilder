﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.FilterBuilder.Models
{
    using Catel;
    using Catel.Reflection;

    public class DictionaryEntryMetadata : IPropertyMetadata
    {
        private string _key;

        private Type _expectedType;

        public DictionaryEntryMetadata()
        {
        }

        public DictionaryEntryMetadata(string key, Type expectedType)
        {
            Argument.IsNotNull(() => key);
            Argument.IsNotNull(() => expectedType);
            _key = key;
            DisplayName = key;
            _expectedType = expectedType;
        }

        public string DisplayName {  get; set; }

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
            Argument.IsOfType(() => instance, typeof(IDictionary<string, object>));
            object result = null;
            (instance as IDictionary<string, object>).TryGetValue(_key, out result);

            return result;
        }

        public TValue GetValue<TValue>(object instance)
        {
            return (TValue) GetValue(instance);
        }

        public void SetValue(object instance, object value)
        {
            Argument.IsOfType(() => instance, typeof(IDictionary<string, object>));
            var dictionary = instance as IDictionary<string, object>;
            if (dictionary.ContainsKey(_key))
            {
                dictionary.Remove(_key);
            }

            dictionary.Add(_key, value);
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
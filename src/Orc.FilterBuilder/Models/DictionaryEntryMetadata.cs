using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.FilterBuilder.Models
{
    using Catel;

    public class DictionaryEntryMetadata : IPropertyMetadata
    {
        private string _key;

        private Type _expectedType;

        public DictionaryEntryMetadata(string key, Type expectedType)
        {
            Argument.IsNotNull(() => key);
            Argument.IsNotNull(() => expectedType);
            _key = key;
            DisplayName = key;
            _expectedType = _expectedType;
        }

        public string DisplayName {  get; set; }

        public string Name
        {
            get { return _key; }
        }

        public Type OwnerType
        {
            get
            {
                return typeof(IDictionary<string, object>);
            }
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
    }
}

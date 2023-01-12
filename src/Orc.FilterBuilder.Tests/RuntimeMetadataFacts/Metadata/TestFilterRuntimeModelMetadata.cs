namespace Orc.FilterBuilder.Tests
{
    using System;

    public class TestFilterRuntimeModelMetadata : IPropertyMetadata
    {
        #region Fields
        private readonly TestAttributeType _attributeType;
        #endregion

        #region Constructors
        public TestFilterRuntimeModelMetadata(TestAttributeType attributeType)
        {
            _attributeType = attributeType;

            OwnerType = typeof (TestFilterRuntimeModel);
            Type = attributeType.Type;
            DisplayName = _attributeType.Name;
        }
        #endregion

        #region Properties
        public string DisplayName { get; set; }

        public string Name => _attributeType.Name;

        public Type OwnerType { get; }

        public Type Type { get; }
        #endregion

        #region IPropertyMetadata Members
        public object GetValue(object instance)
        {
            var testData = instance as TestFilterRuntimeModel;

            return GetAttributeValue(testData, _attributeType);
        }

        public TValue GetValue<TValue>(object instance)
        {
            return (TValue) GetValue(instance);
        }

        public void SetValue(object instance, object value)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods
        private object GetAttributeValue(TestFilterRuntimeModel operation, TestAttributeType attributeType)
        {
            var attributeName = attributeType?.Name;

            if (ReferenceEquals(attributeName, null))
            {
                return null;
            }

            TestAttributeValue value;
            return operation.Attributes.TryGetValue(attributeName, out value) ? value.Value : null;
        }
        #endregion
    }
}

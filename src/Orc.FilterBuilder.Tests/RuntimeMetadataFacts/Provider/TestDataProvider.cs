namespace Orc.FilterBuilder.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    public static class TestDataProvider
    {
        #region Methods
        public static List<TestFilterRuntimeModel> GetInitialCollection()
        {
            var attributeTypes = TestAttributeTypeProvider.AttributeTypes;

            return new List<TestFilterRuntimeModel>
            {
                new TestFilterRuntimeModel
                {
                    Attributes = new Dictionary<string, TestAttributeValue>
                    {
                        {AttributeTypeNames.StringAttribute,new TestAttributeValue("one", attributeTypes[AttributeTypeNames.StringAttribute])},
                        {AttributeTypeNames.IntAttribute, new TestAttributeValue(1, attributeTypes[AttributeTypeNames.IntAttribute])}
                    }
                },

                new TestFilterRuntimeModel
                {
                    Attributes = new Dictionary<string, TestAttributeValue>
                    {
                        {AttributeTypeNames.StringAttribute,new TestAttributeValue("no value", attributeTypes[AttributeTypeNames.StringAttribute])},
                        {AttributeTypeNames.IntAttribute, new TestAttributeValue(0, attributeTypes[AttributeTypeNames.IntAttribute])}
                    }
                },

                new TestFilterRuntimeModel
                {
                    Attributes = new Dictionary<string, TestAttributeValue>
                    {
                        {AttributeTypeNames.StringAttribute,new TestAttributeValue("two", attributeTypes[AttributeTypeNames.StringAttribute])},
                        {AttributeTypeNames.IntAttribute, new TestAttributeValue(2, attributeTypes[AttributeTypeNames.IntAttribute])}
                    }
                }
            };
        }
        #endregion
    }
}
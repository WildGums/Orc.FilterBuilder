// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestFilterRuntimeModelPropertyCollection.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using FilterBuilder.Models;

    public class TestFilterRuntimeModelPropertyCollection : IPropertyCollection
    {
        #region Constructors
        public TestFilterRuntimeModelPropertyCollection(IList<TestAttributeType> attributeTypes)
        {
            Argument.IsNotNull(() => attributeTypes);

            Properties = attributeTypes.Select(x => new TestFilterRuntimeModelMetadata(x))
                .Cast<IPropertyMetadata>()
                .ToList();
        }
        #endregion

        #region Properties
        public List<IPropertyMetadata> Properties { get; }
        #endregion

        #region Methods
        public IPropertyMetadata GetProperty(string propertyName)
        {
            return Properties.FirstOrDefault(x => x.Name == propertyName);
        }
        #endregion
    }
}
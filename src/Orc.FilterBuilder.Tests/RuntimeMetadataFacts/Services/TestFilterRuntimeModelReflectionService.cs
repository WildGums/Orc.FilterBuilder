// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestFilterRuntimeModelReflectionService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Threading;

    public class TestFilterRuntimeModelReflectionService : IReflectionService
    {
        private readonly IList<TestAttributeType> _attributeTypes;

        #region Fields
        private TestFilterRuntimeModelPropertyCollection _propertyCollection;
        #endregion

        #region Constructors
        public TestFilterRuntimeModelReflectionService(IList<TestAttributeType> attributeTypes)
        {
            Argument.IsNotNull(() => attributeTypes);

            _attributeTypes = attributeTypes;
        }
        #endregion

        #region IReflectionService Members
        public IPropertyCollection GetInstanceProperties(Type targetType)
        {
            return _propertyCollection ?? (_propertyCollection = new TestFilterRuntimeModelPropertyCollection(_attributeTypes));
        }

        public Task<IPropertyCollection> GetInstancePropertiesAsync(Type targetType)
        {
            return TaskHelper<IPropertyCollection>.FromResult(GetInstanceProperties(targetType));
        }

        public void ClearCache()
        {
            _propertyCollection = null;
        }
        #endregion
    }
}

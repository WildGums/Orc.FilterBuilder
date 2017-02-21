// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestAttributeTypeProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Tests
{
    using System;
    using System.Collections.Generic;

    public static class TestAttributeTypeProvider
    {
        #region Constants
        public static readonly Dictionary<string, TestAttributeType> AttributeTypes;
        #endregion

        #region Constructors
        static TestAttributeTypeProvider()
        {
            AttributeTypes = new Dictionary<string, TestAttributeType>()
            {
                {"StringAttribute", new TestAttributeType("StringAttribute", typeof (string))},
                {"IntAttribute", new TestAttributeType("IntAttribute", typeof (string))},
                {"DateTimeAttribute", new TestAttributeType("DateTimeAttribute", typeof (string))},
                {"BoolAttribute", new TestAttributeType("BoolAttribute", typeof (string))},
            };
        }
        #endregion
    }
}
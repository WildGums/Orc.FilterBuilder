// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestAttributeType.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Tests
{
    using System;

    public class TestAttributeType
    {
        #region Constructors
        public TestAttributeType(string name, Type type)
        {
            Name = name;
            Type = type;
        }
        #endregion

        #region Properties
        public string Name { get; }
        public Type Type { get; }
        #endregion
    }
}
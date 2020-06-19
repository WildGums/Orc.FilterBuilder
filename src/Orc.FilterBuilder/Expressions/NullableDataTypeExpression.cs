// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTypeExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System.Runtime.Serialization;

    public abstract class NullableDataTypeExpression : DataTypeExpression
    {
        protected NullableDataTypeExpression()
            : base()
        {

        }

        public bool IsNullable { get; set; }
    }
}

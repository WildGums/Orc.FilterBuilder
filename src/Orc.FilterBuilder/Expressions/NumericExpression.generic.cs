namespace Orc.FilterBuilder
{
    using System;

    public abstract class NumericExpression<TValue> : ValueDataTypeExpression<TValue>
        where TValue : struct, IComparable, IFormattable, IConvertible, IComparable<TValue>, IEquatable<TValue>
    {
        protected NumericExpression()
            : base()
        {

        }

        public bool IsDecimal { get; protected set; }
        public bool IsSigned { get; protected set; }
    }
}

namespace Orc.FilterBuilder
{
    using System.Diagnostics;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedLongExpression : NumericExpression<ulong>
    {
        public UnsignedLongExpression()
            : this(true)
        {
        }

        public UnsignedLongExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = false;
            ValueControlType = ValueControlType.UnsignedLong;
        }
    }
}

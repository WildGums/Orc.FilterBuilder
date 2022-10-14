namespace Orc.FilterBuilder
{
    using System.Diagnostics;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedShortExpression : NumericExpression<ushort>
    {
        public UnsignedShortExpression()
            : this(true)
        {
        }

        public UnsignedShortExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = false;
            ValueControlType = ValueControlType.UnsignedShort;
        }
    }
}

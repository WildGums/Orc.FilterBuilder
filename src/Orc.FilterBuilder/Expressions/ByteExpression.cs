namespace Orc.FilterBuilder
{
    using System.Diagnostics;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class ByteExpression : NumericExpression<byte>
    {
        public ByteExpression()
            : this(true)
        {
        }

        public ByteExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = false;
            ValueControlType = ValueControlType.Byte;
        }
    }
}

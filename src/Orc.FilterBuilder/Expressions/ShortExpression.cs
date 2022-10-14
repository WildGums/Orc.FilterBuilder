namespace Orc.FilterBuilder
{
    using System.Diagnostics;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class ShortExpression : NumericExpression<short>
    {
        public ShortExpression()
            : this(true)
        {
        }

        public ShortExpression(bool isNullable)
        {
            IsDecimal = false;
            IsNullable = isNullable;
            IsSigned = true;
            ValueControlType = ValueControlType.Short;
        }
    }
}

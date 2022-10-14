namespace Orc.FilterBuilder
{
    using System.Diagnostics;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class FloatExpression : NumericExpression<float>
    {
        public FloatExpression()
            : this(true)
        {
        }

        public FloatExpression(bool isNullable)
        {
            IsDecimal = true;
            IsNullable = isNullable;
            IsSigned = true;
            ValueControlType = ValueControlType.Float;
        }
    }
}

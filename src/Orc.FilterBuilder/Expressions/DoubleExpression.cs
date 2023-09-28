namespace Orc.FilterBuilder;

using System.Diagnostics;

[DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
public class DoubleExpression : NumericExpression<double>
{
    public DoubleExpression()
        : this(true)
    {
    }

    public DoubleExpression(bool isNullable)
    {
        IsDecimal = true;
        IsNullable = isNullable;
        IsSigned = true;
        ValueControlType = ValueControlType.Double;
    }
}
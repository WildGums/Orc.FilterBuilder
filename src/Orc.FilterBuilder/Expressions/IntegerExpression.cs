namespace Orc.FilterBuilder;

using System.Diagnostics;

[DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
public class IntegerExpression : NumericExpression<int>
{
    public IntegerExpression()
        : this(true)
    {
    }

    public IntegerExpression(bool isNullable)
    {
        IsDecimal = false;
        IsNullable = isNullable;
        IsSigned = true;
        ValueControlType = ValueControlType.Integer;
    }
}

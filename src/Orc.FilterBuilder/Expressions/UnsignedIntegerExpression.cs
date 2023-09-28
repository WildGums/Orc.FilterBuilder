namespace Orc.FilterBuilder;

using System.Diagnostics;

[DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
public class UnsignedIntegerExpression : NumericExpression<uint>
{
    public UnsignedIntegerExpression()
        : this(true)
    {
    }

    public UnsignedIntegerExpression(bool isNullable)
    {
        IsDecimal = false;
        IsNullable = isNullable;
        IsSigned = false;
        ValueControlType = ValueControlType.UnsignedInteger;
    }
}
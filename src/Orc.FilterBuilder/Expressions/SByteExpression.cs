namespace Orc.FilterBuilder;

using System.Diagnostics;

[DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
public class SByteExpression : NumericExpression<sbyte>
{
    public SByteExpression()
        : this(true)
    {
    }

    public SByteExpression(bool isNullable)
    {
        IsDecimal = false;
        IsNullable = isNullable;
        IsSigned = true;
        ValueControlType = ValueControlType.SByte;
    }
}
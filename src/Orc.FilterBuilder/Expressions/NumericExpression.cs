namespace Orc.FilterBuilder;

using System;
using System.Diagnostics;

[DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
public class NumericExpression : NumericExpression<double>
{
    public NumericExpression()
    {
        IsDecimal = true;
        IsSigned = true;
        ValueControlType = ValueControlType.Numeric;
    }

    public NumericExpression(Type type)
        : this()
    {
        IsNullable = type.IsNullable();
    }
}
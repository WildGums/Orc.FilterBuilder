namespace Orc.FilterBuilder;

using System.Diagnostics;
    
[DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
public class LongExpression : NumericExpression<long>
{
    public LongExpression()
        : this(true)
    {
    }

    public LongExpression(bool isNullable)
    {
        IsDecimal = false;
        IsNullable = isNullable;
        IsSigned = true;
        ValueControlType = ValueControlType.Long;
    }
}
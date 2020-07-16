namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class TimeSpanValueExpression : ValueDataTypeExpression<TimeSpan>
    {
        public TimeSpanValueExpression()
            : this(true)
        {
        }

        public TimeSpanValueExpression(bool isNullable)
        {
            IsNullable = isNullable;
            Value = TimeSpan.Zero;
            ValueControlType = ValueControlType.TimeSpan;
        }
    }
}

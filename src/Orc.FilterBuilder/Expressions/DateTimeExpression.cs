namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class DateTimeExpression : ValueDataTypeExpression<DateTime>
    {
        public DateTimeExpression()
            : this(true)
        {
        }

        public DateTimeExpression(bool isNullable)
        {
            IsNullable = isNullable;
            Value = DateTime.Now;
            ValueControlType = ValueControlType.DateTime;
        }
    }
}

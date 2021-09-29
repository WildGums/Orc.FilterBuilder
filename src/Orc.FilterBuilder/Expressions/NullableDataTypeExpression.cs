namespace Orc.FilterBuilder
{
    public abstract class NullableDataTypeExpression : DataTypeExpression
    {
        protected NullableDataTypeExpression()
            : base()
        {

        }

        public bool IsNullable { get; set; }
    }
}

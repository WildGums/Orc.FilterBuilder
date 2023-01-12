namespace Orc.FilterBuilder.Tests
{
    public class TestAttributeValue
    {
        public TestAttributeValue(object value, TestAttributeType attributeType)
        {
            Value = value;
            AttributeType = attributeType;
        }

        #region Properties
        public object Value { get; set; }
        public TestAttributeType AttributeType { get; set; }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{AttributeType.Name}: {Value}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((TestAttributeValue) obj);
        }

        public bool Equals(TestAttributeValue other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(AttributeType, other.AttributeType) && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((AttributeType?.GetHashCode() ?? 0)*397) ^ (Value?.GetHashCode() ?? 0);
            }
        }

        public static bool operator ==(TestAttributeValue left, TestAttributeValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TestAttributeValue left, TestAttributeValue right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
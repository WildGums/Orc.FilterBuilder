namespace Orc.FilterBuilder.Tests;

using System;
using Catel.Data;

public class TestEntity : ModelBase
{
    public string FirstName { get; set; }
    public int Age { get; set; }
    public int? Id { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime? DateOfDeath { get; set; }
    public bool IsActive { get; set; }
    public TimeSpan Duration { get; set; }
    public decimal Price { get; set; }
    public decimal? NullablePrice { get; set; }
    public MyEnum EnumValue { get; set; }
    public MyEnum? NullableEnumValue { get; set; }
    public Description Description { get; set; }

    protected bool Equals(TestEntity other)
    {
        return FirstName == other.FirstName 
               && Age == other.Age 
               && Id == other.Id
               && DateOfBirth.Equals(other.DateOfBirth) 
               && Nullable.Equals(DateOfDeath, other.DateOfDeath) 
               && IsActive == other.IsActive
               && Duration.Equals(other.Duration) 
               && Price == other.Price 
               && NullablePrice == other.NullablePrice 
               && EnumValue == other.EnumValue 
               && NullableEnumValue == other.NullableEnumValue;
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

        return Equals((TestEntity)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(FirstName);
        hashCode.Add(Age);
        hashCode.Add(Id);
        hashCode.Add(DateOfBirth);
        hashCode.Add(DateOfDeath);
        hashCode.Add(IsActive);
        hashCode.Add(Duration);
        hashCode.Add(Price);
        hashCode.Add(NullablePrice);
        hashCode.Add((int)EnumValue);
        hashCode.Add(NullableEnumValue);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(TestEntity left, TestEntity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TestEntity left, TestEntity right)
    {
        return !Equals(left, right);
    }
}

public class Description
{
    public Description(string value)
    {
        Value = value;
    }

    public string Value { get; }
}

public enum MyEnum
{
    EnumValue1,

    EnumValue2,

    SpecialValue
}

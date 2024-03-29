﻿namespace FilterBuilder.Example.Models;

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
}

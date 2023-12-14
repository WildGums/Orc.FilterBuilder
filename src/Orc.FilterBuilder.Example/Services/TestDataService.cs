namespace FilterBuilder.Example.Services;

using System;
using System.Collections.ObjectModel;
using System.IO;
using Models;

public class TestDataService : ITestDataService
{
    private readonly Random _random = new(DateTime.Now.Millisecond);
    private ObservableCollection<TestEntity>? _testItems;

    public ObservableCollection<TestEntity> GetTestItems()
    {
        return _testItems ??= GenerateTestItems();
    }

    public ObservableCollection<TestEntity> GenerateTestItems()
    {
        var items = new ObservableCollection<TestEntity>();
        for (var i = 0; i < 10000; i++)
        {
            items.Add(GenerateRandomEntity());
        }

        return items;
    }

    public TestEntity GenerateRandomEntity()
    {
        var testEntity = new TestEntity
        {
            FirstName = GetRandomString(),
            Age = _random.Next(1, 100),
            Id = _random.Next(10) < 1 ? null : _random.Next(10000),
            DateOfBirth = GetRandomDateTime(),
            DateOfDeath = _random.Next(10) >= 2 ? null : GetRandomDateTime(),
            IsActive = _random.Next(2) == 0,
            Duration = new TimeSpan(_random.Next(3), _random.Next(24), _random.Next(60), _random.Next(60)),
            Price = (decimal)(_random.NextDouble() * 1000 - 500),
            NullablePrice = (_random.Next(10) >= 5 ? (decimal?)(_random.NextDouble() * 1000 - 500) : null),
            EnumValue = (MyEnum)_random.Next(0, 3)
        };

        var next = _random.Next(0, 4);
        testEntity.NullableEnumValue = next == 3 ? null : (MyEnum?)next;

        testEntity.Description = new Description(GetRandomString());
        return testEntity;
    }

    public DateTime GetRandomDateTime()
    {
        return DateTime.Now.AddMinutes((int)(-1 * _random.NextDouble() * 30 * 365 * 24 * 60)); //years*days*hours*minuted
    }

    public string GetRandomString()
    {
        return _random.Next(10) < 1 
            ? null 
            : Path.GetRandomFileName().Replace(".", string.Empty);
    }
}

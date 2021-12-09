// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace FilterBuilder.Example.Services
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using FilterBuilder.Example.Models;

    public class TestDataService : ITestDataService
    {
        #region Fields
        private readonly Random _random;
        private ObservableCollection<TestEntity> _testItems;
        #endregion

        public TestDataService()
        {
            _random = new Random(DateTime.Now.Millisecond);
        }

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
            var testEntity = new TestEntity();
            testEntity.FirstName = GetRandomString();
            testEntity.Age = _random.Next(1, 100);
            testEntity.Id = _random.Next(10) < 1 ? (int?)null : _random.Next(10000);
            testEntity.DateOfBirth = GetRandomDateTime();
            testEntity.DateOfDeath = _random.Next(10) >= 2 ? (DateTime?)null : GetRandomDateTime();
            testEntity.IsActive = _random.Next(2) == 0;
            testEntity.Duration = new TimeSpan(_random.Next(3), _random.Next(24), _random.Next(60), _random.Next(60));
            testEntity.Price = (decimal)(_random.NextDouble() * 1000 - 500);
            testEntity.NullablePrice = (_random.Next(10) >= 5 ? (decimal?)(_random.NextDouble() * 1000 - 500) : null);
            testEntity.EnumValue = (MyEnum)_random.Next(0, 3);

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
            return _random.Next(10) < 1 ? null : Path.GetRandomFileName().Replace(".", "");
        }
    }
}

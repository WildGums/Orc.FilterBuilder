using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Orc.FilterBuilder.Test.NET40
{
	public class MainWindowViewModel
	{
		private readonly Random _random;

		public ObservableCollection<TestEntity> RawItems { get; private set; }
		public ObservableCollection<TestEntity> FilteredItems { get; private set; } 

		public MainWindowViewModel()
		{
			_random = new Random(DateTime.Now.Millisecond);
			RawItems = GenerateTestItems();
			FilteredItems = new ObservableCollection<TestEntity>();
		}

		private ObservableCollection<TestEntity> GenerateTestItems()
		{
			ObservableCollection<TestEntity> items = new ObservableCollection<TestEntity>();
			for (int i = 0; i < 1000; i++)
				items.Add(GenerateRandomEntity());
			return items;
		}

		private TestEntity GenerateRandomEntity()
		{
			TestEntity testEntity = new TestEntity();
			testEntity.FirstName = GetRandomString();
			testEntity.Age = _random.Next(1, 100);
			testEntity.Id = _random.Next(10) < 1
				? (int?) null
				: _random.Next(10000);
			testEntity.DateOfBirth = GetRandomDateTime();
			testEntity.DateOfDeath = _random.Next(10) >= 2
				? (DateTime?) null
				: GetRandomDateTime();
			testEntity.IsActive = _random.Next(2) == 0;
			testEntity.Duration = new TimeSpan(_random.Next(3), _random.Next(24), _random.Next(60), _random.Next(60));
			testEntity.Price = (decimal) (_random.NextDouble()*1000 - 500);
			testEntity.NullablePrice = (_random.Next(10) >= 5
				? (decimal?) (_random.NextDouble()*1000 - 500)
				: null);
			return testEntity;
		}

		private DateTime GetRandomDateTime()
		{
			return DateTime.Now.AddMinutes((int) (-1*_random.NextDouble()*30*365*24*60)); //years*days*hours*minuted
		}

		private string GetRandomString()
		{
			return
				_random.Next(10) < 1
					? null
					: Path.GetRandomFileName().Replace(".", "");
		}
	}
}

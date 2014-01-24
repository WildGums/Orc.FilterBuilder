using System;

namespace Orc.FilterBuilder.Test.NET40
{
	public class TestEntity
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
	}
}

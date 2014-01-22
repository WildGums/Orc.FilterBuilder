using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;

namespace Orc.FilterBuilder.NET40
{
	public class InstanceProperties
	{
		public List<PropertyInfo> Properties { get; private set; }

		public void Init(object instance)
		{
			Properties = new List<PropertyInfo>();
			
			Properties.AddRange(
				instance.GetType().GetProperties()
					.Where(m => m.IsReadable() && m.Type() == typeof (string))
					.ToList());

			Properties.AddRange(
				instance.GetType().GetProperties()
					.Where(m => m.IsReadable() &&
					            (m.Type() == typeof (int) || m.Type() == typeof (int?)))
					.ToList());

			Properties.AddRange(
				instance.GetType().GetProperties()
					.Where(m => m.IsReadable() &&
								(m.Type() == typeof(DateTime) || m.Type() == typeof(DateTime?)))
					.ToList());

			Properties.AddRange(
				instance.GetType().GetProperties()
					.Where(m => m.IsReadable() && m.Type() == typeof (bool))
					.ToList());

			Properties.AddRange(
				instance.GetType().GetProperties()
					.Where(m => m.IsReadable() && m.Type() == typeof (TimeSpan))
					.ToList());

			Properties.AddRange(
				instance.GetType().GetProperties()
					.Where(m => m.IsReadable() &&
					            (m.Type() == typeof (decimal) || m.Type() == typeof (decimal?)))
					.ToList());

			Properties = new List<PropertyInfo>(Properties.OrderBy(m => m.Name));
		}

		public PropertyInfo GetProperty(string name)
		{
			return Properties.Single(pi => pi.Name == name);
		}
	}
}

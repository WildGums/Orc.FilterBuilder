using System;
using System.Reflection;
using Fasterflect;

namespace Orc.FilterBuilder.NET40
{
	public class PropertyExpression : ConditionTreeItem
	{
		private PropertyInfo _property;
		public PropertyInfo Property
		{
			get { return _property; }
			set
			{
				_property = value;
				RaisePropertyChanged(() => Property);
				OnPropertyNameChanged();
			}
		}

		private DataTypeExpression _dataTypeExpression;
		public DataTypeExpression DataTypeExpression
		{
			get { return _dataTypeExpression; }
			set
			{
				_dataTypeExpression = value;
				RaisePropertyChanged(() => DataTypeExpression);
			}
		}

		private void OnPropertyNameChanged()
		{
			if (Property.Type() == typeof (int))
				DataTypeExpression = new IntegerExpression(false);
			else if (Property.Type() == typeof (int?))
				DataTypeExpression = new IntegerExpression(true);
			else if (Property.Type() == typeof (string))
				DataTypeExpression = new StringExpression();
			else if (Property.Type() == typeof (DateTime))
				DataTypeExpression = new DateTimeExpression(false);
			else if (Property.Type() == typeof (DateTime?))
				DataTypeExpression = new DateTimeExpression(true);
			else if (Property.Type() == typeof (bool))
				DataTypeExpression = new BooleanExpression();
			else if (Property.Type() == typeof (TimeSpan))
				DataTypeExpression = new TimeSpanExpression();
			else if (Property.Type() == typeof (decimal))
				DataTypeExpression = new DecimalExpression(false);
			else if (Property.Type() == typeof (decimal?))
				DataTypeExpression = new DecimalExpression(true);
		}

		public override bool CalculateResult(object entity)
		{
			return DataTypeExpression.CalculateResult(Property.Name, entity);
		}

		protected override ConditionTreeItem CopyPlainItem()
		{
			PropertyExpression copied = new PropertyExpression();
			copied.Property = Property;
			copied.DataTypeExpression = DataTypeExpression;
			return copied;
		}
	}
}

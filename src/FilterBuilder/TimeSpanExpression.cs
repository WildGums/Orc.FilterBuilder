using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Fasterflect;

namespace Orc.FilterBuilder
{
	public class TimeSpanExpression : DataTypeExpression
	{
		private const int AverageDaysInYear = 365;
		private const int AverageDaysInMonth = 30;

		public TimeSpan Value { get; set; }
		public List<TimeSpanType> SpanTypes { get; set; }

		private TimeSpanType _selectedSpanType;
		public TimeSpanType SelectedSpanType
		{
			get { return _selectedSpanType; }
			set
			{
				_selectedSpanType = value;
				OnValueParameterChanged();
			}
		}

		private float _amount;
		public float Amount
		{
			get { return _amount; }
			set
			{
				_amount = value;
				OnValueParameterChanged();
			}
		}

		public TimeSpanExpression()
		{
			Conditions = GetValueConditions();
			SelectedCondition = Condition.EqualTo;
			Value = TimeSpan.FromHours(1);
			ValueControlType = ValueControlType.TimeSpan;
			SpanTypes = Enum.GetValues(typeof (TimeSpanType)).Cast<TimeSpanType>().ToList();
			SelectedSpanType = TimeSpanType.Hours;
		}

		private void OnValueParameterChanged()
		{
			switch (SelectedSpanType)
			{
				case TimeSpanType.Years:
					Value = TimeSpan.FromDays(Amount*AverageDaysInYear);
					break;
				case TimeSpanType.Months:
					Value = TimeSpan.FromDays(Amount*AverageDaysInMonth);
					break;
				case TimeSpanType.Days:
					Value = TimeSpan.FromDays(Amount);
					break;
				case TimeSpanType.Hours:
					Value = TimeSpan.FromHours(Amount);
					break;
				case TimeSpanType.Minutes:
					Value = TimeSpan.FromMinutes(Amount);
					break;
				case TimeSpanType.Seconds:
					Value = TimeSpan.FromSeconds(Amount);
					break;
				default:
					throw new NotSupportedException(string.Format("Type '{0}' is not supported.", SelectedSpanType));
			}
		}

		public override bool CalculateResult(string propertyName, object entity)
		{
			TimeSpan entityValue = (TimeSpan)entity.GetPropertyValue(propertyName);
			switch (SelectedCondition)
			{
				case Condition.EqualTo:
					return entityValue == Value;
				case Condition.NotEqualTo:
					return entityValue != Value;
				case Condition.GreaterThan:
					return entityValue > Value;
				case Condition.LessThan:
					return entityValue < Value;
				case Condition.GreaterThanOrEqualTo:
					return entityValue >= Value;
				case Condition.LessThanOrEqualTo:
					return entityValue <= Value;
				default:
					throw new NotSupportedException(string.Format("Condition '{0}' is not supported.", SelectedCondition));
			}
		}

		
	}
}

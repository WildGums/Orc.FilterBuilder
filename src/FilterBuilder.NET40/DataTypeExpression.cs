using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Catel.Data;

namespace Orc.FilterBuilder.NET40
{
	public abstract class DataTypeExpression : ObservableObject
	{
		public List<Condition> Conditions { get; set; }

		private Condition _selectedCondition;
		public Condition SelectedCondition
		{
			get { return _selectedCondition; }
			set
			{
				_selectedCondition = value;
				IsValueRequired = GetIsValueRequired(_selectedCondition);
			}
		}

		private bool _isValueRequired;
		public bool IsValueRequired
		{
			get { return _isValueRequired; }
			set
			{
				_isValueRequired = value;
				RaisePropertyChanged(() => IsValueRequired);
			}
		}

		public ValueControlType ValueControlType { get; set; }

		public abstract bool CalculateResult(string propertyName, object entity);

		protected List<Condition> GetValueConditions()
		{
			return new List<Condition>
			{
				Condition.EqualTo,
				Condition.NotEqualTo,
				Condition.LessThan,
				Condition.GreaterThan,
				Condition.LessThanOrEqualTo,
				Condition.GreaterThanOrEqualTo
			};
		}

		protected List<Condition> GetNullableValueConditions()
		{
			List<Condition> conditions = GetValueConditions();
			conditions.Add(Condition.IsNull);
			conditions.Add(Condition.NotIsNull);
			return conditions;
		}

		protected List<Condition> GetStringConditions()
		{
			return Enum.GetValues(typeof (Condition)).Cast<Condition>().ToList();
		}

		protected List<Condition> GetBooleandConditions()
		{
			return new List<Condition>
			{
				Condition.EqualTo
			};
		}

		protected bool GetIsValueRequired(Condition condition)
		{
			return
				condition == Condition.EqualTo ||
				condition == Condition.NotEqualTo ||
				condition == Condition.GreaterThan ||
				condition == Condition.LessThan ||
				condition == Condition.GreaterThanOrEqualTo ||
				condition == Condition.LessThanOrEqualTo ||
				condition == Condition.Contains ||
				condition == Condition.EndsWith ||
				condition == Condition.StartsWith;
		}
	}
}

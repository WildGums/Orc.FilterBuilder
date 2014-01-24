using System;
using Fasterflect;

namespace Orc.FilterBuilder
{
	public class DateTimeExpression : DataTypeExpression
	{
		public bool IsNullable { get; set; }

		public DateTime Value { get; set; }
		
		public DateTimeExpression(bool isNullable)
		{
			IsNullable = isNullable;
			Conditions = IsNullable
				? GetNullableValueConditions()
				: GetValueConditions();
			SelectedCondition = Condition.EqualTo;
			Value = DateTime.Now;
			ValueControlType = ValueControlType.DateTime;
		}

		public override bool CalculateResult(string propertyName, object entity)
		{
			if (IsNullable)
			{
				DateTime? entityValue = (DateTime?)entity.GetPropertyValue(propertyName);
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
					case Condition.IsNull:
						return entityValue == null;
					case Condition.NotIsNull:
						return entityValue != null;
					default:
						throw new NotSupportedException(string.Format("Condition '{0}' is not supported.", SelectedCondition));
				}
			}
			else
			{
				DateTime entityValue = (DateTime)entity.GetPropertyValue(propertyName);
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
}

using System;
using System.Collections.Generic;
using Fasterflect;

namespace Orc.FilterBuilder.NET40
{
	public class BooleanExpression : DataTypeExpression
	{
		public bool Value { get; set; }
		public List<bool> BooleanValues { get; set; }

		public BooleanExpression()
		{
			Conditions = GetBooleandConditions();
			BooleanValues = new List<bool> {true, false};
			Value = true;
			SelectedCondition = Condition.EqualTo;
			ValueControlType = ValueControlType.Boolean;
		}

		public override bool CalculateResult(string propertyName, object entity)
		{
			bool entityValue = (bool)entity.GetPropertyValue(propertyName);
			switch (SelectedCondition)
			{
				case Condition.EqualTo:
					return entityValue == Value;
				default:
					throw new NotSupportedException(string.Format("Condition '{0}' is not supported.", SelectedCondition));
			}
		}
	}
}

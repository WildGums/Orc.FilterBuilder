// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTypeExpression.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Data;
    using Catel.Runtime.Serialization;

    public abstract class DataTypeExpression : ModelBase
    {
        #region Properties
        [ExcludeFromSerialization]
        public List<Condition> Conditions { get; set; }

        public Condition SelectedCondition { get; set; }

        public bool IsValueRequired { get; set; }

        public ValueControlType ValueControlType { get; set; }
        #endregion

        #region Methods
        private void OnSelectedConditionChanged()
        {
            IsValueRequired = GetIsValueRequired(SelectedCondition);
        }

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
        #endregion
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ConditionHelper
    {
        #region Methods
        public static List<Condition> GetValueConditions()
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

        public static List<Condition> GetNullableValueConditions()
        {
            List<Condition> conditions = GetValueConditions();
            conditions.Add(Condition.IsNull);
            conditions.Add(Condition.NotIsNull);
            return conditions;
        }

        public static List<Condition> GetStringConditions()
        {
            return Enum.GetValues(typeof (Condition)).Cast<Condition>().ToList();
        }

        public static List<Condition> GetBooleanConditions()
        {
            return new List<Condition>
            {
                Condition.EqualTo
            };
        }

        public static bool GetIsValueRequired(Condition condition)
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
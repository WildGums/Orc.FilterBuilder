// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Tests.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Data;
    using FilterBuilder.Models;
    using NUnit.Framework.Constraints;

    public static class FilterSchemeHelper
    {
        public static IEnumerable<TestFilterModel> GenerateMatchingAndNonMatchingCollection()
        {
            List<TestFilterModel> testCollection = new List<TestFilterModel>();

            // Matching value
            testCollection.Add(new TestFilterModel
            {
                BoolProperty = true,
                IntProperty = 50,
                StringProperty = "Test va123lue"
            });

            // Non-Matching value
            testCollection.Add(new TestFilterModel
            {
                BoolProperty = false,
                IntProperty = null,
                StringProperty = "Test va123lue"
            });

            // Matching value
            testCollection.Add(new TestFilterModel
            {
                BoolProperty = true,
                IntProperty = 42,
                StringProperty = "123lue"
            });

            // Non-Matching value
            testCollection.Add(new TestFilterModel
            {
                BoolProperty = true,
                IntProperty = 50,
                StringProperty = "Test value"
            });

            return testCollection;
        }

        public static FilterScheme GenerateFilterScheme()
        {
            var filterScheme = new FilterScheme();
            filterScheme.Title = "Test filter";

            var root = filterScheme.ConditionItems.First();

            root.Items.Add(GenerateConditionGroup());
            root.Items.Add(GenerateConditionGroup());

            return filterScheme;
        }

        public static ConditionGroup GenerateConditionGroup()
        {
            var conditionGroup = new ConditionGroup
            {
                Type = ConditionGroupType.And
            };

            conditionGroup.Items.Add(GenerateStringExpression());
            conditionGroup.Items.Add(GenerateBoolExpression());
            conditionGroup.Items.Add(GenerateIntExpression());

            return conditionGroup;
        }

        public static PropertyExpression GenerateStringExpression()
        {
            var propertyDataManager = PropertyDataManager.Default;
            var typeInfo = propertyDataManager.GetCatelTypeInfo(typeof(TestFilterModel));

            var expression = GenerateExpression<StringExpression>();
            expression.SelectedCondition = Condition.Contains;
            expression.Value = "123";

            var propertyExpression = new PropertyExpression();
            propertyExpression.Property = new PropertyMetadata(typeof(TestFilterModel), typeInfo.GetPropertyData("StringProperty"));
            propertyExpression.DataTypeExpression = expression;

            return propertyExpression;
        }

        public static PropertyExpression GenerateBoolExpression()
        {
            var propertyDataManager = PropertyDataManager.Default;
            var typeInfo = propertyDataManager.GetCatelTypeInfo(typeof(TestFilterModel));

            var expression = GenerateExpression<BooleanExpression>();
            expression.SelectedCondition = Condition.EqualTo;
            expression.Value = true;

            var propertyExpression = new PropertyExpression();
            propertyExpression.Property = new PropertyMetadata(typeof(TestFilterModel), typeInfo.GetPropertyData("BoolProperty"));
            propertyExpression.DataTypeExpression = expression;            

            return propertyExpression;
        }

        public static PropertyExpression GenerateIntExpression()
        {
            var propertyDataManager = PropertyDataManager.Default;
            var typeInfo = propertyDataManager.GetCatelTypeInfo(typeof(TestFilterModel));

            var expression = GenerateExpression<IntegerExpression>();
            expression.SelectedCondition = Condition.GreaterThanOrEqualTo;
            expression.Value = 42;

            var propertyExpression = new PropertyExpression();
            propertyExpression.Property = new PropertyMetadata(typeof(TestFilterModel), typeInfo.GetPropertyData("IntProperty"));
            propertyExpression.DataTypeExpression = expression;            

            return propertyExpression;
        }

        public static TExpression GenerateExpression<TExpression>()
            where TExpression : DataTypeExpression, new()
        {
            var expression = new TExpression();


            return expression;
        }
    }
}
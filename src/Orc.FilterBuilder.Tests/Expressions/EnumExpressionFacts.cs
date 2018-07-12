// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder.Tests.Expressions
{
    using System;
    using FilterBuilder.Models;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class EnumExpressionFacts
    {
        public enum TestEnum
        {
            Enum1,
            Enum2
        }

        private struct NonEnum
        {
        }

        [TestFixture]
        public class The_Constructor
        {
            [Test]
            public void Succeeds()
            {
                new EnumExpression<TestEnum>(false);
            }

            [Test]
            public void Throws_ArgumentException_Whether_The_Generic_Argument_Is_Not_An_Enum()
            {
                Assert.Catch<ArgumentException>(() => new EnumExpression<NonEnum>(false));
            }

            [Test]
            public void Initizalize_The_Value_Property_With_The_First_Enum_Value()
            {
                var enumExpression = new EnumExpression<TestEnum>(false);
                Assert.AreEqual(TestEnum.Enum1, enumExpression.Value);
            }

            [Test]
            [TestCase(TestEnum.Enum1)]
            [TestCase(TestEnum.Enum2)]
            public void Initizalize_The_EnumValues_Property_With_All_Enum_Values(TestEnum value)
            {
                var enumExpression = new EnumExpression<TestEnum>(false);
                Assert.Contains(value, enumExpression.EnumValues);
            }
        }

        [TestFixture]
        public class The_CalculateResult_Method
        {
            [Test]
            public void Throws_NotSupportedException_Whether_The_SelectedCondition_Is_IsNull_And_Is_Not_Nullable()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(null);

                var enumExpression = new EnumExpression<TestEnum>(false) {SelectedCondition = Condition.IsNull};

                Assert.Catch<NotSupportedException>(() => enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Throws_NotSupportedException_Whether_The_SelectedCondition_Is_NotIsNull_And_Is_Not_Nullable()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(null);

                var enumExpression = new EnumExpression<TestEnum>(false) {SelectedCondition = Condition.NotIsNull};

                Assert.Catch<NotSupportedException>(() => enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            [TestCase(Condition.Matches)]
            [TestCase(Condition.DoesNotMatch)]
            [TestCase(Condition.Contains)]
            [TestCase(Condition.DoesNotContain)]
            [TestCase(Condition.StartsWith)]
            [TestCase(Condition.DoesNotStartWith)]
            [TestCase(Condition.EndsWith)]
            [TestCase(Condition.DoesNotEndWith)]
            public void Throws_NotSupportedException_Whether_The_SelectedCondition_Is_Not_Supported(Condition condition)
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum1);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum1,
                    SelectedCondition = condition
                };

                Assert.Catch<NotSupportedException>(() => enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_True_Whether_The_Entity_Property_Value_Is_Equals_To_Value_And_SelectedCondition_Is_EqualsTo()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum1);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum1,
                    SelectedCondition = Condition.EqualTo
                };

                Assert.IsTrue(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_False_Whether_The_Entity_Property_Value_Is_Not_Equals_To_Value_And_SelectedCondition_Is_EqualsTo()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum1);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum2,
                    SelectedCondition = Condition.EqualTo
                };

                Assert.IsFalse(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_True_Whether_The_Entity_Property_Value_Is_Not_Equals_To_Value_And_SelectedCondition_Is_NotEqualTo()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum1);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum2,
                    SelectedCondition = Condition.NotEqualTo
                };

                Assert.IsTrue(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_False_Whether_The_Entity_Property_Value_Is_Equals_To_Value_And_SelectedCondition_Is_NotEqualTo()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum2);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum2,
                    SelectedCondition = Condition.NotEqualTo
                };

                Assert.IsFalse(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_True_Whether_The_Entity_Property_Value_Is_Greater_Than_To_Value_And_SelectedCondition_Is_GreaterThan()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum2);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum1,
                    SelectedCondition = Condition.GreaterThan
                };

                Assert.IsTrue(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_False_Whether_The_Entity_Property_Value_Is_Less_Than_To_Value_And_SelectedCondition_Is_GreaterThan()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum1);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum2,
                    SelectedCondition = Condition.GreaterThan
                };

                Assert.IsFalse(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_True_Whether_The_Entity_Property_Value_Is_Greater_Than_To_Value_And_SelectedCondition_Is_GreaterThanOrEqualTo()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum2);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum1,
                    SelectedCondition = Condition.GreaterThanOrEqualTo
                };

                Assert.IsTrue(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_True_Whether_The_Entity_Property_Value_Is_Equals_To_Value_And_SelectedCondition_Is_GreaterThanOrEqualTo()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum1);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum1,
                    SelectedCondition = Condition.GreaterThanOrEqualTo
                };

                Assert.IsTrue(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_False_Whether_The_Entity_Property_Value_Is_Less_Than_To_Value_And_SelectedCondition_Is_GreaterThanOrEqualTo()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum1);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum2,
                    SelectedCondition = Condition.GreaterThanOrEqualTo
                };

                Assert.IsFalse(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_True_Whether_The_Entity_Property_Value_Is_Less_Than_To_Value_And_SelectedCondition_Is_LessThan()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum1);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum2,
                    SelectedCondition = Condition.LessThan
                };

                Assert.IsTrue(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_False_Whether_The_Entity_Property_Value_Is_Greater_Than_To_Value_And_SelectedCondition_Is_LessThan()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum2);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum1,
                    SelectedCondition = Condition.LessThan
                };

                Assert.IsFalse(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_True_Whether_The_Entity_Property_Value_Is_Less_Than_To_Value_And_SelectedCondition_Is_LessThanOrEqualTo()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum1);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum2,
                    SelectedCondition = Condition.LessThanOrEqualTo
                };

                Assert.IsTrue(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_True_Whether_The_Entity_Property_Value_Is_Equals_To_Value_And_SelectedCondition_Is_LessThanOrEqualTo()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum1);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum1,
                    SelectedCondition = Condition.LessThanOrEqualTo
                };

                Assert.IsTrue(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_False_Whether_The_Entity_Property_Value_Is_Greater_Than_To_Value_And_SelectedCondition_Is_LessThanOrEqualTo()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum2);

                var enumExpression = new EnumExpression<TestEnum>(false)
                {
                    Value = TestEnum.Enum1,
                    SelectedCondition = Condition.LessThanOrEqualTo
                };

                Assert.IsFalse(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_True_Whether_The_Entity_Property_Value_Is_Null_And_SelectedCondition_Is_IsNull()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(null);

                var enumExpression = new EnumExpression<TestEnum>(true)
                {
                    SelectedCondition = Condition.IsNull
                };

                Assert.IsTrue(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_False_Whether_The_Entity_Property_Value_Not_Is_Null_And_SelectedCondition_Is_IsNull()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum2);

                var enumExpression = new EnumExpression<TestEnum>(true)
                {
                    SelectedCondition = Condition.IsNull
                };

                Assert.IsFalse(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_True_Whether_The_Entity_Property_Value_Not_Is_Null_And_SelectedCondition_Is_NotIsNull()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(TestEnum.Enum2);

                var enumExpression = new EnumExpression<TestEnum>(true)
                {
                    SelectedCondition = Condition.NotIsNull
                };

                Assert.IsTrue(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }

            [Test]
            public void Returns_False_Whether_The_Entity_Property_Value_Is_Null_And_SelectedCondition_Is_NotIsNull()
            {
                var entity = new object();
                var propertyMetadataMock = new Mock<IPropertyMetadata>();
                propertyMetadataMock.Setup(metadata => metadata.GetValue(entity)).Returns(null);

                var enumExpression = new EnumExpression<TestEnum>(true)
                {
                    SelectedCondition = Condition.NotIsNull
                };

                Assert.IsFalse(enumExpression.CalculateResult(propertyMetadataMock.Object, entity));
            }
        }
    }
}
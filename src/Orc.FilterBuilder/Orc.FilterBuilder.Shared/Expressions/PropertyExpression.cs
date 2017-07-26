// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using Catel.Reflection;
    using Catel.Runtime.Serialization;
    using Models;
    using Runtime.Serialization;
    using Catel.Data;
    using Catel.Logging;

    [DebuggerDisplay("{Property} = {DataTypeExpression}")]
    [SerializerModifier(typeof(PropertyExpressionSerializerModifier))]
    [ValidateModel(typeof(PropertyExpressionValidator))]
    public class PropertyExpression : ConditionTreeItem
    {
        private static ILog Log = LogManager.GetCurrentClassLogger();

        #region Constructors
        public PropertyExpression()
        {
        }
        #endregion

        #region Properties
        [ExcludeFromSerialization]
        internal string PropertySerializationValue { get; set; }

        public IPropertyMetadata Property { get; set; }

        public DataTypeExpression DataTypeExpression { get; set; }
        #endregion

        #region Methods
        private void OnPropertyChanged()
        {
            var dataTypeExpression = DataTypeExpression;
            if (dataTypeExpression != null)
            {
                dataTypeExpression.PropertyChanged -= OnDataTypeExpressionPropertyChanged;
            }

            if (Property != null)
            {
                var propertyType = Property.Type;
                bool isNullable = propertyType.IsNullableType();
                if (isNullable)
                {
                    propertyType = propertyType.GetNonNullable();
                }

                if (propertyType.IsEnumEx())
                {
                    if (DataTypeExpression != null)
                    {
                        var enumExpressionGenericType = typeof(EnumExpression<>).MakeGenericType(propertyType);
                        if (!enumExpressionGenericType.IsAssignableFromEx(DataTypeExpression.GetType()) || (DataTypeExpression as NullableDataTypeExpression)?.IsNullable != isNullable)
                        {
                            var constructorInfo = enumExpressionGenericType.GetConstructor(new[] {typeof(bool)});
                            DataTypeExpression = (DataTypeExpression) constructorInfo?.Invoke(new object[] {isNullable});
                        }
                    }
                }
                else if (propertyType == typeof(byte))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new ByteExpression(isNullable));
                }
                else if (propertyType == typeof(sbyte))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new SByteExpression(isNullable));
                }
                else if (propertyType == typeof(ushort))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedShortExpression(isNullable));
                }
                else if (propertyType == typeof(short))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new ShortExpression(isNullable));
                }
                else if (propertyType == typeof(uint))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedIntegerExpression(isNullable));
                }
                else if (propertyType == typeof(int))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new IntegerExpression(isNullable));
                }
                else if (propertyType == typeof(ulong))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedLongExpression(isNullable));
                }
                else if (propertyType == typeof(long))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new LongExpression(isNullable));
                }
                else if (propertyType == typeof(string))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new StringExpression());
                }
                else if (propertyType == typeof(DateTime))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new DateTimeExpression(isNullable));
                }
                else if (propertyType == typeof(bool))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new BooleanExpression());
                }
                else if (propertyType == typeof(TimeSpan))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new TimeSpanExpression(isNullable));
                }
                else if (propertyType == typeof(decimal))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new DecimalExpression(isNullable));
                }
                else if (propertyType == typeof(float))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new FloatExpression(isNullable));
                }
                else if (propertyType == typeof(double))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new DoubleExpression(isNullable));
                }
                else
                {
                    Log.Error($"Unable to create data type expression for type '{propertyType}'");
                }
            }

            dataTypeExpression = DataTypeExpression;
            if (dataTypeExpression != null)
            {
                dataTypeExpression.PropertyChanged += OnDataTypeExpressionPropertyChanged;
            }
        }

        private void CreateDataTypeExpressionIfNotCompatible<TDataExpression>(Func<TDataExpression> createFunc)
             where TDataExpression : DataTypeExpression
        {
            var dataTypeExpression = DataTypeExpression;

            if (dataTypeExpression != null && dataTypeExpression is TDataExpression)
            {
                if (dataTypeExpression is NullableDataTypeExpression && typeof(NullableDataTypeExpression).IsAssignableFromEx(typeof(TDataExpression)))
                {
                    var oldDataTypeExpression = dataTypeExpression as NullableDataTypeExpression;
                    var newDataTypeExpression = createFunc() as NullableDataTypeExpression;
                    if (newDataTypeExpression.IsNullable != oldDataTypeExpression.IsNullable)
                    {
                        DataTypeExpression = newDataTypeExpression;
                    }
                }

                return;
            }

            DataTypeExpression = createFunc();
        }

        protected override void OnDeserialized()
        {
            base.OnDeserialized();

            var dataTypeExpression = DataTypeExpression;
            if (dataTypeExpression != null)
            {
                dataTypeExpression.PropertyChanged += OnDataTypeExpressionPropertyChanged;
            }
            else
            {
                OnPropertyChanged();
            }
        }

        private void OnDataTypeExpressionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseUpdated();
        }

        public override bool CalculateResult(object entity)
        {
            if (!IsValid)
            {
                return true;
            }

            return DataTypeExpression.CalculateResult(Property, entity);
        }

        public override string ToString()
        {
            var property = Property;
            if (property == null)
            {
                return string.Empty;
            }

            var dataTypeExpression = DataTypeExpression;
            if (dataTypeExpression == null)
            {
                return string.Empty;
            }

            var dataTypeExpressionString = dataTypeExpression.ToString();
            return string.Format("{0} {1}", property.DisplayName ?? property.Name, dataTypeExpressionString);
        }
        #endregion
    }
}
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

    [DebuggerDisplay("{Property} = {DataTypeExpression}")]
    [SerializerModifier(typeof(PropertyExpressionSerializerModifier))]
    [ValidateModel(typeof(PropertyExpressionValidator))]
    public class PropertyExpression : ConditionTreeItem
    {
        #region Constructors
        public PropertyExpression()
        {
            SuspendValidation = false;
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
                if (Property.Type.IsEnumEx())
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new StringExpression());
                }
                else if (Property.Type == typeof(byte))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new ByteExpression(false));
                }
                else if (Property.Type == typeof(byte?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new ByteExpression(true));
                }
                else if (Property.Type == typeof(sbyte))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new SByteExpression(false));
                }
                else if (Property.Type == typeof(sbyte?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new SByteExpression(true));
                }
                else if (Property.Type == typeof(ushort))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedShortExpression(false));
                }
                else if (Property.Type == typeof(ushort?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedShortExpression(true));
                }
                else if (Property.Type == typeof(short))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new ShortExpression(false));
                }
                else if (Property.Type == typeof(short?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new ShortExpression(true));
                }
                else if (Property.Type == typeof(uint))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedIntegerExpression(false));
                }
                else if (Property.Type == typeof(uint?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedIntegerExpression(true));
                }
                else if (Property.Type == typeof(int))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new IntegerExpression(false));
                }
                else if (Property.Type == typeof(int?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new IntegerExpression(true));
                }
                else if (Property.Type == typeof(ulong))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedLongExpression(false));
                }
                else if (Property.Type == typeof(ulong?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedLongExpression(true));
                }
                else if (Property.Type == typeof(long))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new LongExpression(false));
                }
                else if (Property.Type == typeof(long?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new LongExpression(true));
                }
                else if (Property.Type == typeof(string))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new StringExpression());
                }
                else if (Property.Type == typeof(DateTime))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new DateTimeExpression(false));
                }
                else if (Property.Type == typeof(DateTime?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new DateTimeExpression(true));
                }
                else if (Property.Type == typeof(bool))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new BooleanExpression());
                }
                else if (Property.Type == typeof(TimeSpan))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new TimeSpanExpression());
                }
                else if (Property.Type == typeof(decimal))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new DecimalExpression(false));
                }
                else if (Property.Type == typeof(decimal?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new DecimalExpression(true));
                }
                else if (Property.Type == typeof(float))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new FloatExpression(false));
                }
                else if (Property.Type == typeof(float?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new FloatExpression(true));
                }
                else if (Property.Type == typeof(double))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new DoubleExpression(false));
                }
                else if (Property.Type == typeof(double?))
                {
                    CreateDataTypeExpressionIfNotCompatible(() => new DoubleExpression(true));
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
                if (dataTypeExpression is NullableDataTypeExpression
                    && typeof(NullableDataTypeExpression).IsAssignableFromEx(typeof(TDataExpression)))
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
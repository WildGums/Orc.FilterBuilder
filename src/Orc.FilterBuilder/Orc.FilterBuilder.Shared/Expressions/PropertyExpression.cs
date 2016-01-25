// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyExpression.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
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

    [DebuggerDisplay("{Property} = {DataTypeExpression}")]
    [SerializerModifier(typeof(PropertyExpressionSerializerModifier))]
    public class PropertyExpression : ConditionTreeItem
    {
        #region Properties
        [ExcludeFromSerialization]
        internal string PropertySerializationValue { get; set; }

        public IPropertyMetadata Property { get; set; }

        public DataTypeExpression DataTypeExpression { get; set; }
        #endregion

        #region Methods
        private void OnPropertyChanged()
        {
            if (DataTypeExpression != null)
            {
                DataTypeExpression.PropertyChanged -= OnDataTypeExpressionPropertyChanged;
            }

            if (Property.Type.IsEnumEx())
            {
                CreateDataTypeExpressionIfNotCompatible(() => new StringExpression());
            }
            else if (Property.Type.GetNonNullable() == typeof(uint))
            {
                CreateDataTypeExpressionIfNotCompatible(() => new NumericExpression(Property.Type));
            }
            else if (Property.Type == typeof(int))
            {
                CreateDataTypeExpressionIfNotCompatible(() => new IntegerExpression(false));
            }
            else if (Property.Type == typeof(int?))
            {
                CreateDataTypeExpressionIfNotCompatible(() => new IntegerExpression(true));
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
            else if (Property.Type == typeof(double))
            {
                CreateDataTypeExpressionIfNotCompatible(() => new DoubleExpression(false));
            }
            else if (Property.Type == typeof(double?))
            {
                CreateDataTypeExpressionIfNotCompatible(() => new DoubleExpression(true));
            }

            if (DataTypeExpression != null)
            {
                DataTypeExpression.PropertyChanged += OnDataTypeExpressionPropertyChanged;
            }
        }

        private void CreateDataTypeExpressionIfNotCompatible<TDataExpression>(Func<TDataExpression> createFunc)
            where TDataExpression : DataTypeExpression
        {
            if (DataTypeExpression != null)
            {
                if (DataTypeExpression is TDataExpression)
                {
                    return;
                }
            }

            DataTypeExpression = createFunc();
        }

        protected override void OnDeserialized()
        {
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
            if (Property == null)
            {
                return false;
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

            return string.Format("{0} {1}", property.DisplayName ?? property.Name, DataTypeExpression);
        }
        #endregion
    }
}
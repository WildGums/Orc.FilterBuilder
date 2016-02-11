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
                DataTypeExpression = new StringExpression();
            }
            else if (Property.Type.GetNonNullable() == typeof(uint))
            {
                DataTypeExpression = new NumericExpression(Property.Type);
            }
            else if (Property.Type == typeof(int))
            {
                DataTypeExpression = new IntegerExpression(false);
            }
            else if (Property.Type == typeof(int?))
            {
                DataTypeExpression = new IntegerExpression(true);
            }
            else if (Property.Type == typeof(string))
            {
                DataTypeExpression = new StringExpression();
            }
            else if (Property.Type == typeof(DateTime))
            {
                DataTypeExpression = new DateTimeExpression(false);
            }
            else if (Property.Type == typeof(DateTime?))
            {
                DataTypeExpression = new DateTimeExpression(true);
            }
            else if (Property.Type == typeof(bool))
            {
                DataTypeExpression = new BooleanExpression();
            }
            else if (Property.Type == typeof(TimeSpan))
            {
                DataTypeExpression = new TimeSpanExpression();
            }
            else if (Property.Type == typeof(decimal))
            {
                DataTypeExpression = new DecimalExpression(false);
            }
            else if (Property.Type == typeof(decimal?))
            {
                DataTypeExpression = new DecimalExpression(true);
            }
            else if (Property.Type == typeof(double))
            {
                DataTypeExpression = new DoubleExpression(false);
            }
            else if (Property.Type == typeof(double?))
            {
                DataTypeExpression = new DoubleExpression(true);
            }

            if (DataTypeExpression != null)
            {
                DataTypeExpression.PropertyChanged += OnDataTypeExpressionPropertyChanged;
            }
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
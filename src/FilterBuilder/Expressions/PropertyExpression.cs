// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyExpression.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Reflection;
    using Catel.Runtime.Serialization;
    using Fasterflect;
    using Orc.FilterBuilder.Runtime.Serialization;

    [SerializerModifier(typeof(PropertyExpressionSerializerModifier))]
    public class PropertyExpression : ConditionTreeItem
    {
        #region Properties
        public PropertyInfo Property { get; set; }

        public DataTypeExpression DataTypeExpression { get; set; }
        #endregion

        #region Methods
        private void OnPropertyChanged()
        {
            if (Property.Type() == typeof (int))
            {
                DataTypeExpression = new IntegerExpression(false);
            }
            else if (Property.Type() == typeof (int?))
            {
                DataTypeExpression = new IntegerExpression(true);
            }
            else if (Property.Type() == typeof (string))
            {
                DataTypeExpression = new StringExpression();
            }
            else if (Property.Type() == typeof (DateTime))
            {
                DataTypeExpression = new DateTimeExpression(false);
            }
            else if (Property.Type() == typeof (DateTime?))
            {
                DataTypeExpression = new DateTimeExpression(true);
            }
            else if (Property.Type() == typeof (bool))
            {
                DataTypeExpression = new BooleanExpression();
            }
            else if (Property.Type() == typeof (TimeSpan))
            {
                DataTypeExpression = new TimeSpanExpression();
            }
            else if (Property.Type() == typeof (decimal))
            {
                DataTypeExpression = new DecimalExpression(false);
            }
            else if (Property.Type() == typeof (decimal?))
            {
                DataTypeExpression = new DecimalExpression(true);
            }
        }

        public override bool CalculateResult(object entity)
        {
            return DataTypeExpression.CalculateResult(Property.Name, entity);
        }

        protected override ConditionTreeItem CopyPlainItem()
        {
            var copied = new PropertyExpression();
            copied.Property = Property;
            copied.DataTypeExpression = DataTypeExpression;
            return copied;
        }
        #endregion
    }
}
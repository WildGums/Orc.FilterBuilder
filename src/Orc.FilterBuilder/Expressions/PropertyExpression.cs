// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using Catel.Data;
    using Catel.Logging;
    using Catel.Reflection;
    using Catel.Runtime.Serialization;
    using Models;
    using Runtime.Serialization;

    [DebuggerDisplay("{Property} = {DataTypeExpression}")]
    [SerializerModifier(typeof(PropertyExpressionSerializerModifier))]
    [ValidateModel(typeof(PropertyExpressionValidator))]
    [Serializable]
    public class PropertyExpression : ConditionTreeItem
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        public PropertyExpression()
        {
        }

        protected PropertyExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
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
                CreateDataTypeExpression();
            }

            dataTypeExpression = DataTypeExpression;
            if (dataTypeExpression != null)
            {
                dataTypeExpression.PropertyChanged += OnDataTypeExpressionPropertyChanged;
            }
        }

        private void CreateDataTypeExpression()
        {
            var propertyType = Property.Type;
            var isNullable = propertyType.IsNullableType();
            if (isNullable)
            {
                propertyType = propertyType.GetNonNullable();
            }

            if (TryCreateDataTypeExpressionForEnum(propertyType, isNullable))
            {
                return;
            }

            if (!TryCreateDataTypeExpressionForSystemType(propertyType, isNullable))
            {
                Log.Error($"Unable to create data type expression for type '{propertyType}'");
            }
        }

        private bool TryCreateDataTypeExpressionForSystemType(Type propertyType, bool isNullable)
        {
            switch (propertyType)
            {
                case Type _ when propertyType == typeof(byte):
                    CreateDataTypeExpressionIfNotCompatible(() => new ByteExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(sbyte):
                    CreateDataTypeExpressionIfNotCompatible(() => new SByteExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(ushort):
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedShortExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(short):
                    CreateDataTypeExpressionIfNotCompatible(() => new ShortExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(uint):
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedIntegerExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(int):
                    CreateDataTypeExpressionIfNotCompatible(() => new IntegerExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(ulong):
                    CreateDataTypeExpressionIfNotCompatible(() => new UnsignedLongExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(long):
                    CreateDataTypeExpressionIfNotCompatible(() => new LongExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(string):
                    CreateDataTypeExpressionIfNotCompatible(() => new StringExpression());
                    break;

                case Type _ when propertyType == typeof(DateTime):
                    CreateDataTypeExpressionIfNotCompatible(() => new DateTimeExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(bool):
                    CreateDataTypeExpressionIfNotCompatible(() => new BooleanExpression());
                    break;

                case Type _ when propertyType == typeof(TimeSpan):
                    CreateDataTypeExpressionIfNotCompatible(() => new TimeSpanExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(decimal):
                    CreateDataTypeExpressionIfNotCompatible(() => new DecimalExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(float):
                    CreateDataTypeExpressionIfNotCompatible(() => new FloatExpression(isNullable));
                    break;

                case Type _ when propertyType == typeof(double):
                    CreateDataTypeExpressionIfNotCompatible(() => new DoubleExpression(isNullable));
                    break;

                default:
                    return false;
            }

            return true;
        }

        private bool TryCreateDataTypeExpressionForEnum(Type propertyType, bool isNullable)
        {
            if (!propertyType.IsEnumEx())
            {
                return false;
            }

            if (DataTypeExpression is null)
            {
                return true;
            }

            var enumExpressionGenericType = typeof(EnumExpression<>).MakeGenericType(propertyType);
            if (enumExpressionGenericType.IsAssignableFromEx(DataTypeExpression.GetType())
                && (DataTypeExpression as NullableDataTypeExpression)?.IsNullable == isNullable)
            {
                return true;
            }

            var constructorInfo = enumExpressionGenericType.GetConstructorEx(TypeArray.From<bool>());
            DataTypeExpression = (DataTypeExpression)constructorInfo?.Invoke(new object[] {isNullable});

            return true;
        }

        private void CreateDataTypeExpressionIfNotCompatible<TDataExpression>(Func<TDataExpression> createFunc)
            where TDataExpression : DataTypeExpression
        {
            var dataTypeExpression = DataTypeExpression;

            switch (dataTypeExpression)
            {
                case TDataExpression _ when !(dataTypeExpression is NullableDataTypeExpression) || !typeof(NullableDataTypeExpression).IsAssignableFromEx(typeof(TDataExpression)):
                    return;

                case TDataExpression _:
                    var oldDataTypeExpression = (NullableDataTypeExpression)dataTypeExpression;
                    var newDataTypeExpression = createFunc() as NullableDataTypeExpression;
                    if (newDataTypeExpression is null)
                    {
                        return;
                    }

                    if (newDataTypeExpression.IsNullable != oldDataTypeExpression.IsNullable)
                    {
                        DataTypeExpression = newDataTypeExpression;
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

            if (DataTypeExpression == null)
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

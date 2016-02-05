// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumBinding.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Markup
{
    using System;
    using System.Windows.Markup;
    using Catel;

    public class EnumBinding : MarkupExtension
    {
        #region Fields
        private Type _enumType;
        #endregion

        #region Constructors
        public EnumBinding()
        {
            
        }

        public EnumBinding(Type enumType)
            : this()
        {
            Argument.IsNotNull(() => enumType);

            EnumType = enumType;
        }
        #endregion

        #region Properties
        [ConstructorArgument("enumType")]
        public Type EnumType
        {
            get { return _enumType; }
            private set
            {
                if (_enumType == value)
                {
                    return;
                }

                var enumType = Nullable.GetUnderlyingType(value) ?? value;
                if (enumType.IsEnum == false)
                {
                    throw new ArgumentException("Type must be an Enum.");
                }

                _enumType = value;
            }
        }
        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);
            return enumValues;
        }
    }
}
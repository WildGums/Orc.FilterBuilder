// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionTreeItemConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Converters
{
    using System;
    using System.Windows;
    using Catel.IoC;
    using Catel.MVVM.Converters;
    using Catel.Services;

    public class ConditionTreeItemConverter : ValueConverterBase
    {
        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            switch ((string) parameter)
            {
                case "Group":
                    return value is ConditionGroup ? Visibility.Visible : Visibility.Collapsed;

                case "Expression":
                    return value is PropertyExpression ? Visibility.Visible : Visibility.Collapsed;

                default:
                    throw new NotSupportedException(string.Format(this.GetDependencyResolver().Resolve<ILanguageService>().GetString("FilterBuilder_Exception_Message_Parameter0IsNotSupported_Pattern"), parameter));
            }
        }
        #endregion
    }
}
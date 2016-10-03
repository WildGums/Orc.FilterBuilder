// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTypeExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder
{
    using System.Linq.Expressions;

    using Catel.Data;

    using Orc.FilterBuilder.Models;

    public abstract class DataTypeExpression : ModelBase
    {
        #region Constructors
        protected DataTypeExpression()
        {
            IsValueRequired = true;
        }
        #endregion



        #region Properties
        public Condition SelectedCondition { get; set; }

        public bool IsValueRequired { get; set; }

        public ValueControlType ValueControlType { get; set; }
        #endregion



        #region Methods
        private void OnSelectedConditionChanged()
        {
            IsValueRequired = ConditionHelper.GetIsValueRequired(SelectedCondition);
        }

        public abstract bool CalculateResult(IPropertyMetadata propertyMetadata, object entity);

        /// <summary>
        ///   Converts <see cref="ConditionTreeItem"/> to a LINQ <see cref="Expression"/>
        /// </summary>
        /// <param name="propertyExpr">LINQ <see cref="MemberExpression"/>.</param>
        /// <returns>LINQ Expression.</returns>
        public abstract Expression ToLinqExpression(Expression propertyExpr);
        #endregion
    }
}
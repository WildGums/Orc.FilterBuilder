// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTypeExpression.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using Catel.Data;

    public abstract class DataTypeExpression : ModelBase
    {
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

        public abstract bool CalculateResult(string propertyName, object entity);
        #endregion
    }
}
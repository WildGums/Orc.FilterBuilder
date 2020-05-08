// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTypeExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System.Runtime.Serialization;
    using Catel.Data;

    public abstract class DataTypeExpression : ModelBase
    {
        protected DataTypeExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        protected DataTypeExpression()
        {
            IsValueRequired = true;
        }

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
        #endregion
    }
}

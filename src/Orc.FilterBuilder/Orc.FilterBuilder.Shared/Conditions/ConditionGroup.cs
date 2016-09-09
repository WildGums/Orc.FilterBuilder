// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionGroup.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    public class ConditionGroup : ConditionTreeItem
    {
        #region Constructors
        public ConditionGroup()
        {
            Type = ConditionGroupType.And;
        }
        #endregion



        #region Properties
        public ConditionGroupType Type { get; set; }
        #endregion



        #region Methods
        public override bool CalculateResult(object entity)
        {
            if (!Items.Any())
            {
                return true;
            }

            if (Type == ConditionGroupType.And)
            {
                return Items.Aggregate(true, (current, item) => current && item.CalculateResult(entity));
            }
            else
            {
                return Items.Aggregate(false, (current, item) => current || item.CalculateResult(entity));
            }
        }

        /// <summary>
        ///   Converts <see cref="ConditionTreeItem"/> to a LINQ <see cref="Expression"/>
        /// </summary>
        /// <param name="parameterExpr">LINQ <see cref="ParameterExpression"/>.</param>
        /// <returns>LINQ Expression.</returns>
        public override Expression ToLinqExpression(Expression parameterExpr)
        {
            if (!Items.Any())
            {
                return Expression.Constant(true, typeof(bool));
            }

            var lastExpr = Items.First().ToLinqExpression(parameterExpr);

            if (Items.Count <= 1)
            {
                return lastExpr;
            }

            for (int i = 1; i < Items.Count; i++)
            {
                var rightExpr = Items[i].ToLinqExpression(parameterExpr);

                switch (Type)
                {
                    case ConditionGroupType.And:
                        lastExpr = Expression.AndAlso(lastExpr, rightExpr);
                        break;

                    case ConditionGroupType.Or:
                        lastExpr = Expression.OrElse(lastExpr, rightExpr);
                        break;

                    default:
                        throw new InvalidOperationException($"Unsupported ConditionGroupType: {Type}");
                }
            }

            return lastExpr;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            var groupType = Type.ToString().ToLower();

            var itemCount = Items.Count;
            if (itemCount > 1)
            {
                stringBuilder.Append("(");
            }

            for (var i = 0; i < itemCount; i++)
            {
                if (i > 0)
                {
                    stringBuilder.AppendFormat(" {0} ", groupType);
                }

                var item = Items[i];
                var itemString = item.ToString();
                stringBuilder.Append(itemString);
            }

            if (itemCount > 1)
            {
                stringBuilder.Append(")");
            }

            return stringBuilder.ToString();
        }
        #endregion
    }
}
namespace Orc.FilterBuilder
{
    using System;
    using System.Linq;
    using System.Text;

    public class ConditionGroup : ConditionTreeItem
    {
        public ConditionGroup()
        {
            Type = ConditionGroupType.And;
        }

        public ConditionGroupType Type { get; set; }

        public override bool CalculateResult(object entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

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
    }
}

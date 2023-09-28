namespace Orc.FilterBuilder;

using System;
using System.Linq;
using System.Text;

public class ConditionGroup : ConditionTreeItem
{
    public ConditionGroupType Type { get; set; } = ConditionGroupType.And;

    public override bool CalculateResult(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (!Items.Any())
        {
            return true;
        }

        return Type == ConditionGroupType.And
            ? Items.Aggregate(true, (current, item) => current && item.CalculateResult(entity)) 
            : Items.Aggregate(false, (current, item) => current || item.CalculateResult(entity));
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        var groupType = Type.ToString().ToLower();

        var itemCount = Items.Count;
        if (itemCount > 1)
        {
            stringBuilder.Append('(');
        }

        for (var i = 0; i < itemCount; i++)
        {
            if (i > 0)
            {
                stringBuilder.Append($" {groupType} ");
            }

            var item = Items[i];
            var itemString = item.ToString();
            stringBuilder.Append(itemString);
        }

        if (itemCount > 1)
        {
            stringBuilder.Append(')');
        }

        return stringBuilder.ToString();
    }
}

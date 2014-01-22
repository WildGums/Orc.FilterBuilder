using System.Collections.Generic;
using System.Linq;

namespace Orc.FilterBuilder.NET40
{
	public class ConditionGroup : ConditionTreeItem
	{
		public List<ConditionGroupType> GroupTypes { get; set; }
		public ConditionGroupType Type { get; set; }

		public ConditionGroup()
		{
			GroupTypes = new List<ConditionGroupType>
			{
				ConditionGroupType.And,
				ConditionGroupType.Or
			};
			Type = ConditionGroupType.And;
		}

		public override bool CalculateResult(object entity)
		{
			if (!Items.Any())
				return true;

			if (Type == ConditionGroupType.And)
				return Items.Aggregate(true, (current, item) => current && item.CalculateResult(entity));
			else
				return Items.Aggregate(false, (current, item) => current || item.CalculateResult(entity));
		}

		protected override ConditionTreeItem CopyPlainItem()
		{
			ConditionGroup copiedGroup = new ConditionGroup();
			copiedGroup.Type = Type;
			return copiedGroup;
		}

		public override string ToString()
		{
			return Type.ToString();
		}

	}
}

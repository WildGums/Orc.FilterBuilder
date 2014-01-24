using System.Collections.ObjectModel;
using Catel.Data;

namespace Orc.FilterBuilder
{
	public abstract class ConditionTreeItem : ObservableObject
	{
		public ConditionTreeItem Parent { get; set; }
		public ObservableCollection<ConditionTreeItem> Items { get; set; }

		protected ConditionTreeItem()
		{
			Items = new ObservableCollection<ConditionTreeItem>();
		}

		public abstract bool CalculateResult(object entity);

		public ConditionTreeItem Copy()
		{
			ConditionTreeItem copiedItem = CopyPlainItem();
			foreach (ConditionTreeItem childItem in Items)
			{
				ConditionTreeItem copiedChild = childItem.Copy();
				copiedItem.Items.Add(copiedChild);
				copiedChild.Parent = copiedItem;
			}
			return copiedItem;
		}

		protected abstract ConditionTreeItem CopyPlainItem();
	}
}

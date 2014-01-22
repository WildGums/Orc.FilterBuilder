using System.Collections.ObjectModel;
using System.Linq;
using Catel.Data;

namespace Orc.FilterBuilder.NET40
{
	public class FilterScheme : ObservableObject
	{
		private string _title;
		public string Title
		{
			get { return _title; }
			set
			{
				_title = value;
				RaisePropertyChanged(()=>Title);
			}
		}

		public ConditionTreeItem Root{get { return ConditionItems.First(); }}

		public ObservableCollection<ConditionTreeItem> ConditionItems { get; private set; }

		public FilterScheme(string title, ConditionTreeItem root)
		{
			Title = title;
			ConditionItems = new ObservableCollection<ConditionTreeItem>();
			ConditionItems.Add(root);
		}

		public FilterScheme()
		{
			Title = string.Empty;
			ConditionItems = new ObservableCollection<ConditionTreeItem>();
			ConditionItems.Add(new ConditionGroup());
		}

		public bool CalculateResult(object entity)
		{
			return Root.CalculateResult(entity);
		}

		public FilterScheme Copy()
		{
			return new FilterScheme(Title, Root.Copy());
		}

		public void Update(FilterScheme otherScheme)
		{
			Title = otherScheme.Title;
			ConditionItems.Clear();
			ConditionItems.Add(otherScheme.Root);
		}
	}
}

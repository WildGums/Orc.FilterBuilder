using System;
using System.Linq;
using System.Windows;
using Catel.Data;
using Catel.MVVM;

namespace Orc.FilterBuilder.NET40
{
	public class EditFilterViewModel : ObservableObject
	{
		private FilterScheme _originalFilterScheme;
		private Action _successCallback;

		public EditFilterView View { get; set; }

		private FilterScheme _filterScheme;
		public FilterScheme FilterScheme
		{
			get { return _filterScheme; }
			set
			{
				_filterScheme = value;
				RaisePropertyChanged(() => FilterScheme);
			}
		}

		public string Title
		{
			get { return FilterScheme.Title; }
			set
			{
				FilterScheme.Title = value;
				RaisePropertyChanged(() => Title);
				SaveCommand.RaiseCanExecuteChanged();
			}
		}

		private InstanceProperties _instanceProperties;
		public InstanceProperties InstanceProperties
		{
			get { return _instanceProperties; }
			set
			{
				_instanceProperties = value;
				RaisePropertyChanged(() => InstanceProperties);
			}
		}

		public Command SaveCommand { get; private set; }
		public Command CancelCommand { get; private set; }
		public Command<ConditionGroup> AddGroupCommand { get; private set; }
		public Command<ConditionGroup> AddExpressionCommand { get; private set; }
		public Command<ConditionTreeItem> DeleteConditionItem { get; private set; }

		public EditFilterViewModel()
		{
			SaveCommand = new Command(OnSave, () => !string.IsNullOrEmpty(FilterScheme.Title));
			CancelCommand = new Command(OnCancel);
			AddGroupCommand = new Command<ConditionGroup>(OnAddGroup);
			AddExpressionCommand = new Command<ConditionGroup>(OnAddEpression);
			DeleteConditionItem = new Command<ConditionTreeItem>(OnDeleteCondition);
		}

		public void Run(FilterScheme filterScheme, InstanceProperties instanceProperties, Action successCallback)
		{
			_originalFilterScheme = filterScheme;
			FilterScheme copiedFilterScheme = _originalFilterScheme.Copy();
			FilterScheme = copiedFilterScheme;
			InstanceProperties = instanceProperties;
			_successCallback = successCallback;
		}

		private void OnSave()
		{
			_originalFilterScheme.Update(FilterScheme);
			_successCallback();
			(View.Parent as Window).Close();
		}

		private void OnCancel()
		{
			(View.Parent as Window).Close();
		}

		private void OnDeleteCondition(ConditionTreeItem item)
		{
			if (item.Parent == null)
				item.Items.Clear();
			else
				item.Parent.Items.Remove(item);
		}

		private void OnAddEpression(ConditionGroup group)
		{
			PropertyExpression propertyExpression = new PropertyExpression();
			propertyExpression.Property = InstanceProperties.Properties.FirstOrDefault();
			group.Items.Add(propertyExpression);
			propertyExpression.Parent = group;
		}

		private void OnAddGroup(ConditionGroup group)
		{
			ConditionGroup newGroup = new ConditionGroup();
			group.Items.Add(newGroup);
			newGroup.Parent = group;
		}
	}
}

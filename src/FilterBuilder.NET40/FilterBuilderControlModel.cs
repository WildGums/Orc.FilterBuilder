using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;

namespace Orc.FilterBuilder.NET40
{
	public class FilterBuilderControlModel : ObservableObject
	{
		private InstanceProperties _instanceProperties;

		public ObservableCollection<FilterScheme> FilterSchemes { get; private set; }

		public FilterBuilderControl View { get; set; }

		private FilterScheme _selectedFilterScheme;
		public FilterScheme SelectedFilterScheme
		{
			get { return _selectedFilterScheme; }
			set
			{
				_selectedFilterScheme = value;
				RaisePropertyChanged(() => SelectedFilterScheme);
			}
		}

		public Command NewSchemeCommand { get; private set; }
		public Command EditSchemeCommand { get; private set; }
		public Command ApplySchemeCommand { get; private set; }

		public FilterBuilderControlModel()
		{
			FilterSchemes = new ObservableCollection<FilterScheme>();
			NewSchemeCommand = new Command(OnNewScheme);
			EditSchemeCommand = new Command(OnEditScheme, () => SelectedFilterScheme != null);
			ApplySchemeCommand = new Command(OnApplyScheme, () => SelectedFilterScheme != null);
		}

		public void OnNewScheme()
		{
			FilterScheme newFilterScheme = new FilterScheme();
			EditFilterView editFilterView = new EditFilterView();
			editFilterView.ViewModel.Run(newFilterScheme, _instanceProperties,
				() =>
				{
					FilterSchemes.Add(newFilterScheme);
					SelectedFilterScheme = newFilterScheme;
					Save();
				});
			Window window = new Window();
			window.Title = "New Filter";
			window.Content = editFilterView;
			window.ShowDialog();
		}

		public void OnEditScheme()
		{
			EditFilterView editFilterView = new EditFilterView();
			editFilterView.ViewModel.Run(SelectedFilterScheme, _instanceProperties, Save);
			Window window = new Window();
			window.Title = "Edit Filter";
			window.Content = editFilterView;
			window.ShowDialog();
		}

		private void Save()
		{
			new FilterSchemeManager().Save(FilterSchemes);
		}

		public void OnApplyScheme()
		{
			View.FilteredCollection.Clear();
			foreach (object item in View.RawCollection)
				if (SelectedFilterScheme.CalculateResult(item))
					View.FilteredCollection.Add(item);
		}

		public void InitProperties()
		{
			IEnumerator enumerator = View.RawCollection.GetEnumerator();
			if (enumerator.MoveNext())
			{
				_instanceProperties = new InstanceProperties();
				_instanceProperties.Init(enumerator.Current);

				FilterSchemes.Clear();
				FilterSchemes.AddRange(new FilterSchemeManager().Load(_instanceProperties));
				SelectedFilterScheme = FilterSchemes.FirstOrDefault();
			}

		}

	}
}

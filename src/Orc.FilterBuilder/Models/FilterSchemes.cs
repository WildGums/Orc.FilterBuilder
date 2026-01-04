namespace Orc.FilterBuilder;

using System.Collections.ObjectModel;
using Catel;
using Catel.Data;

public class FilterSchemes : ModelBase
{
    public ObservableCollection<FilterScheme> Schemes { get; private set; } = new();
}

﻿namespace Orc.FilterBuilder;

using System.Collections.ObjectModel;
using Catel;
using Catel.Data;
using Catel.Runtime.Serialization;

public class FilterSchemes : ModelBase
{
    private object? _scope;

    [ExcludeFromSerialization]
    public object? Scope
    {
        get { return _scope; }
        set
        {
            if (ObjectHelper.AreEqual(_scope, value))
            {
                return;
            }

            _scope = value;

            RaisePropertyChanged(nameof(Scope));

            foreach (var filterScheme in Schemes)
            {
                filterScheme.Scope = Scope;
            }
        }
    }

    public ObservableCollection<FilterScheme> Schemes { get; private set; } = new();
}

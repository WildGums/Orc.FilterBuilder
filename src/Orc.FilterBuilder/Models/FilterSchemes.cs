// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemes.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using Catel;
    using Catel.Data;
    using Catel.Runtime.Serialization;

    public class FilterSchemes : ModelBase
    {
        private object _scope;

        public FilterSchemes()
        {
            Schemes = new ObservableCollection<FilterScheme>();
        }

        [ExcludeFromSerialization]
        public object Scope
        {
            get { return _scope; }
            set
            {
                if (!ObjectHelper.AreEqual(_scope, value))
                {
                    _scope = value;

                    RaisePropertyChanged(nameof(Scope));

                    foreach (var filterScheme in Schemes)
                    {
                        filterScheme.Scope = Scope;
                    }
                }
            }
        }

        public ObservableCollection<FilterScheme> Schemes { get; private set; }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemes.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{   
    using System.Collections.ObjectModel;
    using Catel.Data;
    using Catel.Runtime.Serialization;

    public class FilterSchemes : ModelBase
    {
        private object _scope;

        #region Constructors
        public FilterSchemes()
        {
            Schemes = new ObservableCollection<FilterScheme>();
        }
        #endregion

        #region Properties
        [ExcludeFromSerialization]
        public object Scope
        {
            get { return _scope; }
            set
            {
                _scope = value;

                foreach (var filterScheme in Schemes)
                {
                    filterScheme.Scope = Scope;
                }
            }
        }

        public ObservableCollection<FilterScheme> Schemes { get; private set; }
        #endregion
    }
}
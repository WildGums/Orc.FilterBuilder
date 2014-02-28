// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemes.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{   
    using System.Collections.ObjectModel;
    using Catel.Data;

    public class FilterSchemes : ModelBase
    {
        #region Constructors
        public FilterSchemes()
        {
            Schemes = new ObservableCollection<FilterScheme>();
        }
        #endregion

        #region Properties
        public ObservableCollection<FilterScheme> Schemes { get; private set; }
        #endregion
    }
}
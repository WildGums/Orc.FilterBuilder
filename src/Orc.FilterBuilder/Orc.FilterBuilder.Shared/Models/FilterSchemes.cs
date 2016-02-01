// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemes.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{   
    using System.Collections.ObjectModel;
    using Catel.Data;
    using Catel.Runtime.Serialization;

    public class FilterSchemes : ModelBase
    {
        #region Constructors
        public FilterSchemes()
        {
            Schemes = new ObservableCollection<FilterScheme>();
        }
        #endregion

        #region Properties
        [ExcludeFromSerialization]
        public object Tag { get; set; }

        public ObservableCollection<FilterScheme> Schemes { get; private set; }
        #endregion
    }
}
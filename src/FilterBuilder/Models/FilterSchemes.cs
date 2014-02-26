// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemes.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{
    using System.Collections.Generic;
    using Catel.Data;

    public class FilterSchemes : ModelBase
    {
        #region Constructors
        public FilterSchemes()
        {
            Schemes = new List<FilterScheme>();
        }
        #endregion

        #region Properties
        public List<FilterScheme> Schemes { get; private set; }
        #endregion
    }
}
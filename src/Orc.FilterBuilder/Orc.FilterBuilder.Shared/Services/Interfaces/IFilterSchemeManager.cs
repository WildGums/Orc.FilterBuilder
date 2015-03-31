// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilterSchemeManager.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System;
    using Orc.FilterBuilder.Models;

    public interface IFilterSchemeManager
    {
        #region Properties
        bool AutoSave { get; set; }
        FilterSchemes FilterSchemes { get; }
        #endregion

        event EventHandler<EventArgs> Updated;
        event EventHandler<EventArgs> Loaded;
        event EventHandler<EventArgs> Saved;

        #region Methods
        void UpdateFilters();
        void Load(string fileName = null);
        void Save(string fileName = null);
        #endregion
    }
}
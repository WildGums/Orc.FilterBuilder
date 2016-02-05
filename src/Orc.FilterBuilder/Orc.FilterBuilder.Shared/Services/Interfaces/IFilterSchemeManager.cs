// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilterSchemeManager.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System;
    using System.Threading.Tasks;
    using Orc.FilterBuilder.Models;

    public interface IFilterSchemeManager
    {
        #region Properties
        bool AutoSave { get; set; }
        FilterSchemes FilterSchemes { get; }
        object Tag { get; set; }
        #endregion

        event EventHandler<EventArgs> Updated;
        event EventHandler<EventArgs> Loaded;
        event EventHandler<EventArgs> Saved;

        #region Methods
        void UpdateFilters();        
        void Load(string fileName = null);
        Task<bool> LoadAsync(string fileName = null);
        void Save(string fileName = null);
        #endregion
    }
}
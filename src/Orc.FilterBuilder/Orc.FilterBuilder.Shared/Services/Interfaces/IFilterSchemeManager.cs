// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilterSchemeManager.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
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
        #endregion

        event EventHandler<EventArgs> Updated;
        event EventHandler<EventArgs> Loaded;
        event EventHandler<EventArgs> Saved;

        #region Methods
        void UpdateFilters();
        [ObsoleteEx(ReplacementTypeOrMember = "LoadAsync", TreatAsErrorFromVersion = "1.0", RemoveInVersion = "2.0")]
        void Load(string fileName = null);
        Task<bool> LoadAsync(string fileName = null);
        void Save(string fileName = null);
        #endregion
    }
}
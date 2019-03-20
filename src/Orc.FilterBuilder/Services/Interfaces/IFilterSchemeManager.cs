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
        object Scope { get; set; }
        #endregion

        event EventHandler<EventArgs> Updated;
        event EventHandler<EventArgs> Loaded;
        event EventHandler<EventArgs> Saved;

        #region Methods
        Task UpdateFiltersAsync();
        Task<bool> LoadAsync(string fileName = null);
        Task SaveAsync(string fileName = null);

        [ObsoleteEx(ReplacementTypeOrMember = "UpdateFiltersAsync", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        void UpdateFilters();

        [ObsoleteEx(ReplacementTypeOrMember = "IFilterSerializationService.LoadFiltersAsync", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        void Load(string fileName = null);

        [ObsoleteEx(ReplacementTypeOrMember = "IFilterSerializationService.SaveFiltersAsync", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        void Save(string fileName = null);
        #endregion
    }
}

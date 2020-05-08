namespace Orc.FilterBuilder
{
    using System;
    using System.Threading.Tasks;

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
        #endregion
    }
}

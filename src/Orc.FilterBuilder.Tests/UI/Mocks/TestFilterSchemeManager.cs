namespace Orc.FilterBuilder.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.Caching;
    using Catel.Services;
    using Catel.Threading;

    public class TestFilterSchemeManager : IFilterSchemeManager
    {
        private readonly CacheStorage<object, FilterSchemes> _filterSchemes = new();

        public TestFilterSchemeManager()
        {
            TaskHelper.RunAndWaitAsync(async () => await UpdateFiltersAsync());
        }

        public bool AutoSave { get; set; }

        public FilterSchemes FilterSchemes =>
            _filterSchemes.GetFromCacheOrFetch(Scope, () => FilterBuilderControlTestData.GetSchemes(Scope));
        public object Scope { get; set; }

        public async Task UpdateFiltersAsync()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }

        public async Task<bool> LoadAsync(string fileName = null)
        {
            Loaded?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public async Task SaveAsync(string fileName = null)
        {
            Saved?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> Updated;
        public event EventHandler<EventArgs> Loaded;
        public event EventHandler<EventArgs> Saved;
    }
}

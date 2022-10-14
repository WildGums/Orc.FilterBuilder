namespace Orc.FilterBuilder
{
    using System;
    using System.Collections;

    public class FilterSchemeEditInfo
    {
        public FilterSchemeEditInfo(FilterScheme filterScheme, IEnumerable rawCollection, bool allowLivePreview, bool enableAutoCompletion)
        {
            ArgumentNullException.ThrowIfNull(filterScheme);
            ArgumentNullException.ThrowIfNull(rawCollection);

            FilterScheme = filterScheme;
            RawCollection = rawCollection;
            AllowLivePreview = allowLivePreview;
            EnableAutoCompletion = enableAutoCompletion;
        }

        public FilterScheme FilterScheme { get; private set; }

        public IEnumerable RawCollection { get; private set; }

        public bool AllowLivePreview { get; private set; }

        public bool EnableAutoCompletion { get; private set; }
    }
}

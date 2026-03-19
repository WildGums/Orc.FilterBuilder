namespace Orc.FilterBuilder;

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

    public FilterScheme FilterScheme { get; init; }

    public IEnumerable RawCollection { get; init; }

    public bool AllowLivePreview { get; init; }

    public bool EnableAutoCompletion { get; init; }
}

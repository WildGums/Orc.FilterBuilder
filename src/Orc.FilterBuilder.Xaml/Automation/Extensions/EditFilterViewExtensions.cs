namespace Orc.FilterBuilder.Automation;

using System;
using System.Collections.Generic;

public static class EditFilterViewExtensions
{
    public static void Initialize<T>(this EditFilterView target, IEnumerable<T> testCollection)
    {
        ArgumentNullException.ThrowIfNull(target);

        var model = target.Current;

        target.Clear();
        model.FilterSchemeEditInfo = new FilterSchemeEditInfo
        (
            FilterSchemeBuilder.StartGroup<T>().ToFilterScheme(),
            testCollection,
            true,
            true
        );
    }
}
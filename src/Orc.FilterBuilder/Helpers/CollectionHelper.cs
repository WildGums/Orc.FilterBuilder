namespace Orc.FilterBuilder;

using System;
using System.Collections;
using Catel.Logging;

public static class CollectionHelper
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    public static Type? GetTargetType(IEnumerable collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        var enumerator = collection.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            Log.Debug("Collection does not contain items, cannot get any type information");
            return null;
        }

        var firstElement = enumerator.Current;
        return firstElement?.GetType();
    }
}
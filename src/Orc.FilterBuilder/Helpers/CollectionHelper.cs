namespace Orc.FilterBuilder;

using System;
using System.Collections;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public static class CollectionHelper
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(CollectionHelper));

    public static Type? GetTargetType(IEnumerable collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        var enumerator = collection.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            Logger.LogDebug("Collection does not contain items, cannot get any type information");
            return null;
        }

        var firstElement = enumerator.Current;
        return firstElement?.GetType();
    }
}

namespace Orc.FilterBuilder.Tests;

using System.Collections.Generic;

public static class FilterBuilderControlTestData
{
    public static List<string> AvailableScopes = new()
    {
        TestScopeWith0CustomRecords,
        TestScopeWith3CustomRecords,
        TestScopeWith5CustomRecords
    };

    public const string TestScopeWith0CustomRecords = nameof(TestScopeWith0CustomRecords);
    public const string TestScopeWith3CustomRecords = nameof(TestScopeWith3CustomRecords);
    public const string TestScopeWith5CustomRecords = nameof(TestScopeWith5CustomRecords);

    public static FilterSchemes GetSchemes(object scope)
    {
        if (Equals(scope, TestScopeWith3CustomRecords))
        {
            var filterSchemes = new FilterSchemes();

            var schemes = filterSchemes.Schemes;

            schemes.Add(new FilterScheme(typeof(TestEntity)) { Title = "First" });
            schemes.Add(new FilterScheme(typeof(TestEntity)) { Title = "Second" });
            schemes.Add(new FilterScheme(typeof(TestEntity)) { Title = "Third" });

            return filterSchemes;
        }

        if (Equals(scope, TestScopeWith5CustomRecords))
        {
            var filterSchemes = new FilterSchemes();

            var schemes = filterSchemes.Schemes;

            schemes.Add(new FilterScheme(typeof(TestEntity)) { Title = "One" });
            schemes.Add(new FilterScheme(typeof(TestEntity)) { Title = "Two" });
            schemes.Add(new FilterScheme(typeof(TestEntity)) { Title = "Three" });
            schemes.Add(new FilterScheme(typeof(TestEntity)) { Title = "Four" });
            schemes.Add(new FilterScheme(typeof(TestEntity)) { Title = "Five" });

            return filterSchemes;
        }

        return new FilterSchemes();
    }
}

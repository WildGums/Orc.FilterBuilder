namespace Orc.FilterBuilder
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Orc.FilterBuilder.Models;

    [DebuggerDisplay("{Title}")]
    public class FilterGroup
    {
        public FilterGroup(string title, IEnumerable<FilterScheme> filterSchemes)
        {
            Title = title;
            FilterSchemes = new List<FilterScheme>();

            if (filterSchemes != null)
            {
                FilterSchemes.AddRange(filterSchemes);
            }
        }

        public string Title { get; private set; }

        public List<FilterScheme> FilterSchemes { get; private set; }
    }
}

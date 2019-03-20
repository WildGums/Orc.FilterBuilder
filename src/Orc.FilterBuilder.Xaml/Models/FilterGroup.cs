namespace Orc.FilterBuilder
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Orc.FilterBuilder.Models;

    [DebuggerDisplay("{Title}")]
    public class FilterGroup
    {
        public FilterGroup()
        {
            FilterSchemes = new List<FilterScheme>();
        }

        public string Title { get; set; }

        public List<FilterScheme> FilterSchemes { get; protected internal set; }
    }
}

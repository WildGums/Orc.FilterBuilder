namespace Orc.FilterBuilder.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Models;

    public class AutoCompletionService : IAutoCompletionService
    {
        public string[] GetAutoCompleteValues(string propertyName, string filter, IEnumerable source, IMetadataProvider metadataProvider)
        {
            Argument.IsNotNull("source", source);
            Argument.IsNotNull(() => metadataProvider);
            
            
            var propertyValues = new List<string>();

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                try
                {
                    // Filter items directly
                    propertyValues.AddRange(from x in source.OfType<string>()
                                            select x);
                }
                catch (Exception)
                {
                    // Swallow
                }
            }
            else
            {
                var propertyMetadata = metadataProvider.Properties.FirstOrDefault(p => p.Name == propertyName);
                propertyValues.AddRange(from x in source.Cast<object>()
                                        select ObjectToStringHelper.ToString(propertyMetadata.GetValue(x)));
            }

            propertyValues = propertyValues.Where(x => !string.Equals(x, "null")).Distinct().ToList();

            var filteredValues = propertyValues;

            if (!string.IsNullOrEmpty(filter))
            {
                filteredValues = filteredValues.Where(x => x.Contains(filter)).ToList();
            }

            var orderedPropertyValues = filteredValues.GroupBy(x => x).Select(g => new
            {
                Value = g.Key,
                Count = g.Select(x => x).Distinct().Count()
            }).OrderBy(x => x.Count).Select(x => x.Value).Take(10);

            return orderedPropertyValues.OrderBy(x => x).ToArray();
        }
    }
}

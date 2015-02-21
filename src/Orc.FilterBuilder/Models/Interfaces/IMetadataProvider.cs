using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.FilterBuilder.Models
{
    public interface IMetadataProvider
    {
        List<IPropertyMetadata> Properties { get; }

        bool IsAssignableFromEx(IMetadataProvider otherProvider);

        string SerializeState();

        void DeserializeState(string contentAsString);
    }
}

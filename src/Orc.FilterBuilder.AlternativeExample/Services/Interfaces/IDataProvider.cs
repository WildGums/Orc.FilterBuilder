namespace Orc.FilterBuilder.AlternativeExample.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Models;

    public interface IDataProvider
    {
        IEnumerable<object> GetData();

        IMetadataProvider GetMetadata();
    }
}

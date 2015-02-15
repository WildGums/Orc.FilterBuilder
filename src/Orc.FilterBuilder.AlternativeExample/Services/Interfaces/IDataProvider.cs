namespace Orc.FilterBuilder.AlternativeExample.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IDataProvider
    {
        IEnumerable<object> GetData();

        Dictionary<string, Type> GetMetadata();
    }
}

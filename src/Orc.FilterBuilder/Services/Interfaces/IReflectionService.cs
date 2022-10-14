namespace Orc.FilterBuilder
{
    using System;

    public interface IReflectionService
    {
        IPropertyCollection GetInstanceProperties(Type targetType);

        void ClearCache();
    }
}

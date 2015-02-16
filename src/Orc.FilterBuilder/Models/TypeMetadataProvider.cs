namespace Orc.FilterBuilder.Models
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Catel.IoC;
    using Services;

    public class TypeMetadataProvider: IMetadataProvider
    {
        private Type _targetType;

        private IReflectionService _reflectionService;

        public TypeMetadataProvider(Type targetType) : this(targetType, ServiceLocator.Default.ResolveType<IReflectionService>())
        {
        }
        
        public TypeMetadataProvider(Type targetType, IReflectionService reflectionService)
        {
            Argument.IsNotNull(() => targetType);
            Argument.IsNotNull(() => reflectionService);
            _targetType = targetType;
            _reflectionService = reflectionService;
        }

        public List<IPropertyMetadata> Properties
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAssignableFromEx(IMetadataProvider otherProvider)
        {
            throw new NotImplementedException();
        }
    }
}

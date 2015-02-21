namespace Orc.FilterBuilder.Models
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Catel;
    using Catel.IoC;
    using Catel.Reflection;
    using Catel.Runtime.Serialization;
    using Runtime.Serialization;
    using Services;
    
    public class TypeMetadataProvider: IMetadataProvider
    {
        private IReflectionService _reflectionService;

        public TypeMetadataProvider()
        {
            _reflectionService = ServiceLocator.Default.ResolveType<IReflectionService>();
        }

        public TypeMetadataProvider(Type targetType) : this(targetType, ServiceLocator.Default.ResolveType<IReflectionService>())
        {
        }
        
        public TypeMetadataProvider(Type targetType, IReflectionService reflectionService)
        {
            Argument.IsNotNull(() => targetType);
            Argument.IsNotNull(() => reflectionService);
            
            TargetType = targetType;
            _reflectionService = reflectionService;
        }

        public Type TargetType { get; private set; }

        [XmlIgnore]
        public List<IPropertyMetadata> Properties
        {
            get
            {
                return _reflectionService.GetInstanceProperties(TargetType).Properties;
            }
        }

        public bool IsAssignableFromEx(IMetadataProvider otherProvider)
        {
            if (!(otherProvider is TypeMetadataProvider))
            {
                return false;
            }

            var secondProvider = otherProvider as TypeMetadataProvider;
            return TargetType.IsAssignableFromEx(secondProvider.TargetType);
        }

        public string SerializeState()
        {
            return TargetType.FullName;
        }

        public void DeserializeState(string contentAsString)
        {
            TargetType = TypeCache.GetType(contentAsString);
        }
    }
}

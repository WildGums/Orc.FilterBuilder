// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeSerializerModifier.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Runtime.Serialization
{
    using Catel.Data;
    using Catel.Runtime.Serialization;
    using Models;

    public class PropertyExpressionSerializerModifier : SerializerModifierBase<PropertyExpression>
    {
        private const string Separator = "||";

        public override void SerializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(memberValue.Name, "Property"))
            {
                var propertyInfo = memberValue.Value as IPropertyMetadata;
                if (propertyInfo != null)
                {
                    memberValue.Value = string.Format("{0}{1}{2}", propertyInfo.OwnerType.FullName, Separator, propertyInfo.Name);
                }
            }
        }

        public override void DeserializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(memberValue.Name, "Property"))
            {
                var propertyName = memberValue.Value as string;
                if (propertyName != null)
                {
                    // We need to delay this
                    ((PropertyExpression)context.Model).PropertySerializationValue = propertyName;
                }
            }
        }

        public override void OnDeserialized(ISerializationContext context, IModel model)
        {
            base.OnDeserialized(context, model);

            var propertyExpression = (PropertyExpression) context.Model;
            propertyExpression.EnsureIntegrity();
        }
    }
}
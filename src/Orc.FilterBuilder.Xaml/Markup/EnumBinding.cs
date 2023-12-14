namespace Orc.FilterBuilder.Markup;

using System;
using System.Windows.Markup;

public class EnumBinding : MarkupExtension
{
    private Type? _enumType;

    public EnumBinding()
    {
            
    }

    public EnumBinding(Type enumType)
        : this()
    {
        ArgumentNullException.ThrowIfNull(enumType);

        EnumType = enumType;
    }

    [ConstructorArgument("enumType")]
    public Type? EnumType
    {
        get => _enumType;
        private set
        {
            if (value is null)
            {
                _enumType = null;
                return;
            }

            if (_enumType == value)
            {
                return;
            }

            var enumType = Nullable.GetUnderlyingType(value) ?? value;
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type must be an Enum");
            }

            _enumType = value;
        }
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        var enumType = EnumType;
        if (enumType is null)
        {
            return null;
        }

        var enumValues = Enum.GetValues(enumType);
        return enumValues;
    }
}

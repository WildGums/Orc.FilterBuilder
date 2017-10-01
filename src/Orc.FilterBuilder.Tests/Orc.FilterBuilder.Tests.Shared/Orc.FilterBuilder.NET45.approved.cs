[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.InteropServices.ComVisibleAttribute(false)]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.5", FrameworkDisplayName=".NET Framework 4.5")]
[assembly: System.Windows.ThemeInfoAttribute(System.Windows.ResourceDictionaryLocation.None, System.Windows.ResourceDictionaryLocation.SourceAssembly)]


public class static ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.FilterBuilder.Behaviors
{
    
    public class DisableSelectionInTreeView : Catel.Windows.Interactivity.BehaviorBase<System.Windows.Controls.TreeView>
    {
        public DisableSelectionInTreeView() { }
        protected override void OnAssociatedObjectLoaded() { }
        protected override void OnAssociatedObjectUnloaded() { }
    }
}
namespace Orc.FilterBuilder
{
    
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class BooleanExpression : Orc.FilterBuilder.DataTypeExpression
    {
        public static readonly Catel.Data.PropertyData BooleanValuesProperty;
        public static readonly Catel.Data.PropertyData ValueProperty;
        public BooleanExpression() { }
        [Catel.Runtime.Serialization.ExcludeFromSerializationAttribute()]
        public System.Collections.Generic.List<bool> BooleanValues { get; set; }
        public bool Value { get; set; }
        public override bool CalculateResult(Orc.FilterBuilder.Models.IPropertyMetadata propertyMetadata, object entity) { }
        public override string ToString() { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class ByteExpression : Orc.FilterBuilder.NumericExpression<byte>
    {
        public ByteExpression() { }
        public ByteExpression(bool isNullable) { }
    }
    public class static CollectionHelper
    {
        public static System.Type GetTargetType(System.Collections.IEnumerable collection) { }
    }
    public enum Condition
    {
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_Contains")]
        Contains = 0,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_StartsWith")]
        StartsWith = 1,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_EndsWith")]
        EndsWith = 2,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_EqualTo")]
        EqualTo = 3,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_NotEqualTo")]
        NotEqualTo = 4,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_GreaterThan")]
        GreaterThan = 5,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_LessThan")]
        LessThan = 6,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_GreaterThanOrEqualTo")]
        GreaterThanOrEqualTo = 7,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_LessThanOrEqualTo")]
        LessThanOrEqualTo = 8,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_IsEmpty")]
        IsEmpty = 9,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_NotIsEmpty")]
        NotIsEmpty = 10,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_IsNull")]
        IsNull = 11,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_NotIsNull")]
        NotIsNull = 12,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_Matches")]
        Matches = 13,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_DoesNotMatch")]
        DoesNotMatch = 14,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_DoesNotContain")]
        DoesNotContain = 15,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_DoesNotStartWith")]
        DoesNotStartWith = 16,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_DoesNotEndWith")]
        DoesNotEndWith = 17,
    }
    public class static ConditionExtensions
    {
        public static string Humanize(this Orc.FilterBuilder.Condition condition) { }
    }
    public class ConditionGroup : Orc.FilterBuilder.ConditionTreeItem
    {
        public static readonly Catel.Data.PropertyData TypeProperty;
        public ConditionGroup() { }
        public Orc.FilterBuilder.ConditionGroupType Type { get; set; }
        public override bool CalculateResult(object entity) { }
        public override string ToString() { }
    }
    public enum ConditionGroupType
    {
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_And")]
        And = 0,
        [Catel.ComponentModel.DisplayNameAttribute("FilterBuilder_Or")]
        Or = 1,
    }
    public class static ConditionHelper
    {
        public static System.Collections.Generic.List<Orc.FilterBuilder.Condition> GetBooleanConditions() { }
        public static bool GetIsValueRequired(Orc.FilterBuilder.Condition condition) { }
        public static System.Collections.Generic.List<Orc.FilterBuilder.Condition> GetNullableValueConditions() { }
        public static System.Collections.Generic.List<Orc.FilterBuilder.Condition> GetStringConditions() { }
        public static System.Collections.Generic.List<Orc.FilterBuilder.Condition> GetValueConditions() { }
    }
    public abstract class ConditionTreeItem : Catel.Data.ValidatableModelBase
    {
        public static readonly Catel.Data.PropertyData IsValidProperty;
        public static readonly Catel.Data.PropertyData ItemsProperty;
        public static readonly Catel.Data.PropertyData ParentProperty;
        protected ConditionTreeItem() { }
        [Catel.Data.ExcludeFromValidationAttribute()]
        [Catel.Runtime.Serialization.ExcludeFromSerializationAttribute()]
        public bool IsValid { get; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.FilterBuilder.ConditionTreeItem> Items { get; }
        [Catel.Runtime.Serialization.ExcludeFromSerializationAttribute()]
        public Orc.FilterBuilder.ConditionTreeItem Parent { get; set; }
        public event System.EventHandler<System.EventArgs> Updated;
        public abstract bool CalculateResult(object entity);
        public override bool Equals(object obj) { }
        public override int GetHashCode() { }
        protected override void OnDeserialized() { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
        protected override void OnValidated(Catel.Data.IValidationContext validationContext) { }
        protected void RaiseUpdated() { }
    }
    public class static ConditionTreeItemExtensions
    {
        public static bool IsRoot(this Orc.FilterBuilder.ConditionTreeItem item) { }
    }
    public abstract class DataTypeExpression : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData IsValueRequiredProperty;
        public static readonly Catel.Data.PropertyData SelectedConditionProperty;
        public static readonly Catel.Data.PropertyData ValueControlTypeProperty;
        protected DataTypeExpression() { }
        public bool IsValueRequired { get; set; }
        public Orc.FilterBuilder.Condition SelectedCondition { get; set; }
        public Orc.FilterBuilder.ValueControlType ValueControlType { get; set; }
        public abstract bool CalculateResult(Orc.FilterBuilder.Models.IPropertyMetadata propertyMetadata, object entity);
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class DateTimeExpression : Orc.FilterBuilder.ValueDataTypeExpression<System.DateTime>
    {
        public DateTimeExpression() { }
        public DateTimeExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class DecimalExpression : Orc.FilterBuilder.NumericExpression<decimal>
    {
        public DecimalExpression() { }
        public DecimalExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class DoubleExpression : Orc.FilterBuilder.NumericExpression<double>
    {
        public DoubleExpression() { }
        public DoubleExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class EnumExpression<TEnum> : Orc.FilterBuilder.NullableDataTypeExpression
        where TEnum :  struct
    {
        public static readonly Catel.Data.PropertyData ValueProperty;
        public EnumExpression(bool isNullable) { }
        public System.Collections.Generic.List<TEnum> EnumValues { get; }
        public TEnum Value { get; set; }
        public virtual bool CalculateResult(Orc.FilterBuilder.Models.IPropertyMetadata propertyMetadata, object entity) { }
        public override string ToString() { }
    }
    public enum FilterBuilderMode
    {
        Collection = 0,
        FilteringFunction = 1,
    }
    public class static FilterSchemeExtensions
    {
        [MethodTimer.TimeAttribute()]
        public static void Apply(this Orc.FilterBuilder.Models.FilterScheme filterScheme, System.Collections.IEnumerable rawCollection, System.Collections.IList filteredCollection) { }
        public static void EnsureIntegrity(this Orc.FilterBuilder.Models.FilterScheme filterScheme, Orc.FilterBuilder.Services.IReflectionService reflectionService) { }
        public static void EnsureIntegrity(this Orc.FilterBuilder.ConditionTreeItem conditionTreeItem, Orc.FilterBuilder.Services.IReflectionService reflectionService) { }
        public static void EnsureIntegrity(this Orc.FilterBuilder.PropertyExpression propertyExpression, Orc.FilterBuilder.Services.IReflectionService reflectionService) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class FloatExpression : Orc.FilterBuilder.NumericExpression<float>
    {
        public FloatExpression() { }
        public FloatExpression(bool isNullable) { }
    }
    public class static InstancePropertyHelper
    {
        public static bool IsSupportedType(this Orc.FilterBuilder.Models.IPropertyMetadata property) { }
        public static bool IsSupportedType(this System.Reflection.PropertyInfo property) { }
        public static bool IsSupportedType(this System.Type type) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class IntegerExpression : Orc.FilterBuilder.NumericExpression<int>
    {
        public IntegerExpression() { }
        public IntegerExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class LongExpression : Orc.FilterBuilder.NumericExpression<long>
    {
        public LongExpression() { }
        public LongExpression(bool isNullable) { }
    }
    public abstract class NullableDataTypeExpression : Orc.FilterBuilder.DataTypeExpression
    {
        public static readonly Catel.Data.PropertyData IsNullableProperty;
        protected NullableDataTypeExpression() { }
        public bool IsNullable { get; set; }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class NumericExpression : Orc.FilterBuilder.NumericExpression<double>
    {
        public NumericExpression() { }
        public NumericExpression(System.Type type) { }
    }
    public abstract class NumericExpression<TValue> : Orc.FilterBuilder.ValueDataTypeExpression<TValue>
        where TValue :  struct, System.IComparable, System.IFormattable, System.IConvertible, System.IComparable<>, System.IEquatable<>
    {
        public static readonly Catel.Data.PropertyData IsDecimalProperty;
        public static readonly Catel.Data.PropertyData IsSignedProperty;
        protected NumericExpression() { }
        public bool IsDecimal { get; set; }
        public bool IsSigned { get; set; }
    }
    [Catel.Data.ValidateModelAttribute(typeof(Orc.FilterBuilder.PropertyExpressionValidator))]
    [Catel.Runtime.Serialization.SerializerModifierAttribute(typeof(Orc.FilterBuilder.Runtime.Serialization.PropertyExpressionSerializerModifier))]
    [System.Diagnostics.DebuggerDisplayAttribute("{Property} = {DataTypeExpression}")]
    public class PropertyExpression : Orc.FilterBuilder.ConditionTreeItem
    {
        public static readonly Catel.Data.PropertyData DataTypeExpressionProperty;
        public static readonly Catel.Data.PropertyData PropertyProperty;
        public static readonly Catel.Data.PropertyData PropertySerializationValueProperty;
        public PropertyExpression() { }
        public Orc.FilterBuilder.DataTypeExpression DataTypeExpression { get; set; }
        public Orc.FilterBuilder.Models.IPropertyMetadata Property { get; set; }
        public override bool CalculateResult(object entity) { }
        protected override void OnDeserialized() { }
        public override string ToString() { }
    }
    public class PropertyExpressionValidator : Catel.Data.ValidatorBase<Orc.FilterBuilder.PropertyExpression>
    {
        public PropertyExpressionValidator() { }
        protected override void ValidateFields(Orc.FilterBuilder.PropertyExpression instance, System.Collections.Generic.List<Catel.Data.IFieldValidationResult> validationResults) { }
    }
    public class static RegexHelper
    {
        public static bool IsValid(string pattern) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class SByteExpression : Orc.FilterBuilder.NumericExpression<sbyte>
    {
        public SByteExpression() { }
        public SByteExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class ShortExpression : Orc.FilterBuilder.NumericExpression<short>
    {
        public ShortExpression() { }
        public ShortExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class StringExpression : Orc.FilterBuilder.DataTypeExpression
    {
        public static readonly Catel.Data.PropertyData ValueProperty;
        public StringExpression() { }
        public string Value { get; set; }
        public override bool CalculateResult(Orc.FilterBuilder.Models.IPropertyMetadata propertyMetadata, object entity) { }
        public override string ToString() { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class TimeSpanExpression : Orc.FilterBuilder.NullableDataTypeExpression
    {
        public static readonly Catel.Data.PropertyData AmountProperty;
        public static readonly Catel.Data.PropertyData SelectedSpanTypeProperty;
        public static readonly Catel.Data.PropertyData SpanTypesProperty;
        public static readonly Catel.Data.PropertyData ValueProperty;
        public TimeSpanExpression() { }
        public TimeSpanExpression(bool isNullable) { }
        public float Amount { get; set; }
        public Orc.FilterBuilder.TimeSpanType SelectedSpanType { get; set; }
        [Catel.Runtime.Serialization.ExcludeFromSerializationAttribute()]
        public System.Collections.Generic.List<Orc.FilterBuilder.TimeSpanType> SpanTypes { get; set; }
        public System.TimeSpan Value { get; set; }
        public override bool CalculateResult(Orc.FilterBuilder.Models.IPropertyMetadata propertyMetadata, object entity) { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
        public override string ToString() { }
    }
    public enum TimeSpanType
    {
        Years = 0,
        Months = 1,
        Days = 2,
        Hours = 3,
        Minutes = 4,
        Seconds = 5,
    }
    public class static TreeViewItemExtensions
    {
        public static int GetDepth(this System.Windows.Controls.TreeViewItem item) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedIntegerExpression : Orc.FilterBuilder.NumericExpression<uint>
    {
        public UnsignedIntegerExpression() { }
        public UnsignedIntegerExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedLongExpression : Orc.FilterBuilder.NumericExpression<ulong>
    {
        public UnsignedLongExpression() { }
        public UnsignedLongExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedShortExpression : Orc.FilterBuilder.NumericExpression<ushort>
    {
        public UnsignedShortExpression() { }
        public UnsignedShortExpression(bool isNullable) { }
    }
    public enum ValueControlType
    {
        Text = 0,
        DateTime = 1,
        Boolean = 2,
        TimeSpan = 3,
        Decimal = 4,
        Double = 5,
        Integer = 6,
        Numeric = 7,
        UnsignedInteger = 8,
        Byte = 9,
        SByte = 10,
        Short = 11,
        UnsignedShort = 12,
        Long = 13,
        UnsignedLong = 14,
        Float = 15,
        Enum = 16,
    }
    public abstract class ValueDataTypeExpression<TValue> : Orc.FilterBuilder.NullableDataTypeExpression
        where TValue :  struct, System.IComparable, System.IFormattable, System.IConvertible, System.IComparable<>, System.IEquatable<>
    {
        public static readonly Catel.Data.PropertyData ValueProperty;
        protected ValueDataTypeExpression() { }
        public TValue Value { get; set; }
        public override bool CalculateResult(Orc.FilterBuilder.Models.IPropertyMetadata propertyMetadata, object entity) { }
        public override string ToString() { }
    }
}
namespace Orc.FilterBuilder.Conditions
{
    
    public class static ConditionsLinqExtensions
    {
        public static System.Linq.Expressions.Expression<System.Func<T, bool>> BuildLambda<T>(this Orc.FilterBuilder.ConditionTreeItem conditionTreeItem) { }
    }
}
namespace Orc.FilterBuilder.Converters
{
    
    public class ConditionTreeItemConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public ConditionTreeItemConverter() { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
    public class DataTypeExpressionToConditionsConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public DataTypeExpressionToConditionsConverter() { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
    public class IsCurrentFilterSchemeToCollapsingVisibilityConverter : System.Windows.Data.IMultiValueConverter
    {
        public IsCurrentFilterSchemeToCollapsingVisibilityConverter() { }
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) { }
        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) { }
    }
    public class LeftMarginMultiplierConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public LeftMarginMultiplierConverter() { }
        public double Length { get; set; }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
    public class ObjectToValueConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public ObjectToValueConverter(Orc.FilterBuilder.Models.IPropertyMetadata propertyMetadata) { }
        public ObjectToValueConverter() { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
    public class TriggerConverter : System.Windows.Data.IMultiValueConverter
    {
        public TriggerConverter() { }
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) { }
        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) { }
    }
    public class ValueControlTypeVisibilityConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public ValueControlTypeVisibilityConverter() { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
}
namespace Orc.FilterBuilder.Markup
{
    
    public class EnumBinding : System.Windows.Markup.MarkupExtension
    {
        public EnumBinding() { }
        public EnumBinding(System.Type enumType) { }
        [System.Windows.Markup.ConstructorArgumentAttribute("enumType")]
        public System.Type EnumType { get; }
        public override object ProvideValue(System.IServiceProvider serviceProvider) { }
    }
}
namespace Orc.FilterBuilder.Models
{
    
    [Catel.Runtime.Serialization.SerializerModifierAttribute(typeof(Orc.FilterBuilder.Runtime.Serialization.FilterSchemeSerializerModifier))]
    public class FilterScheme : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData ConditionItemsProperty;
        public static readonly Catel.Data.PropertyData HasInvalidConditionItemsProperty;
        public static readonly Catel.Data.PropertyData TargetTypeProperty;
        public static readonly Catel.Data.PropertyData TitleProperty;
        public FilterScheme() { }
        public FilterScheme(System.Type targetType) { }
        public FilterScheme(System.Type targetType, string title) { }
        public FilterScheme(System.Type targetType, string title, Orc.FilterBuilder.ConditionTreeItem root) { }
        public System.Collections.ObjectModel.ObservableCollection<Orc.FilterBuilder.ConditionTreeItem> ConditionItems { get; }
        public bool HasInvalidConditionItems { get; }
        [Catel.Runtime.Serialization.ExcludeFromSerializationAttribute()]
        public Orc.FilterBuilder.ConditionTreeItem Root { get; }
        [Catel.Runtime.Serialization.ExcludeFromSerializationAttribute()]
        public object Scope { get; set; }
        public System.Type TargetType { get; }
        public string Title { get; set; }
        public event System.EventHandler<System.EventArgs> Updated;
        public bool CalculateResult(object entity) { }
        public override bool Equals(object obj) { }
        public override int GetHashCode() { }
        protected override void OnDeserialized() { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
        protected void RaiseUpdated() { }
        public override string ToString() { }
        public void Update(Orc.FilterBuilder.Models.FilterScheme otherScheme) { }
    }
    public class FilterSchemeEditInfo
    {
        public FilterSchemeEditInfo(Orc.FilterBuilder.Models.FilterScheme filterScheme, System.Collections.IEnumerable rawCollection, bool allowLivePreview, bool enableAutoCompletion) { }
        public bool AllowLivePreview { get; }
        public bool EnableAutoCompletion { get; }
        public Orc.FilterBuilder.Models.FilterScheme FilterScheme { get; }
        public System.Collections.IEnumerable RawCollection { get; }
    }
    public class FilterSchemes : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData SchemesProperty;
        public FilterSchemes() { }
        public System.Collections.ObjectModel.ObservableCollection<Orc.FilterBuilder.Models.FilterScheme> Schemes { get; }
        [Catel.Runtime.Serialization.ExcludeFromSerializationAttribute()]
        public object Scope { get; set; }
    }
    public class InstanceProperties : Orc.FilterBuilder.Models.IPropertyCollection
    {
        public InstanceProperties(System.Type type) { }
        public System.Collections.Generic.List<Orc.FilterBuilder.Models.IPropertyMetadata> Properties { get; }
        public Orc.FilterBuilder.Models.IPropertyMetadata GetProperty(string propertyName) { }
    }
    public interface IPropertyCollection
    {
        System.Collections.Generic.List<Orc.FilterBuilder.Models.IPropertyMetadata> Properties { get; }
        Orc.FilterBuilder.Models.IPropertyMetadata GetProperty(string propertyName);
    }
    public interface IPropertyMetadata
    {
        string DisplayName { get; set; }
        string Name { get; }
        System.Type OwnerType { get; }
        System.Type Type { get; }
        object GetValue(object instance);
        TValue GetValue<TValue>(object instance);
        void SetValue(object instance, object value);
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{OwnerType}.{Name}")]
    public class PropertyMetadata : Orc.FilterBuilder.Models.IPropertyMetadata
    {
        public PropertyMetadata(System.Type ownerType, System.Reflection.PropertyInfo propertyInfo) { }
        public PropertyMetadata(System.Type ownerType, Catel.Data.PropertyData propertyData) { }
        public string DisplayName { get; set; }
        public string Name { get; }
        public System.Type OwnerType { get; }
        public System.Type Type { get; }
        public override bool Equals(object obj) { }
        public override int GetHashCode() { }
        public object GetValue(object instance) { }
        public TValue GetValue<TValue>(object instance) { }
        public void SetValue(object instance, object value) { }
    }
}
namespace Orc.FilterBuilder.Runtime.Serialization
{
    
    public class FilterSchemeSerializerModifier : Catel.Runtime.Serialization.SerializerModifierBase<Orc.FilterBuilder.Models.FilterScheme>
    {
        public FilterSchemeSerializerModifier() { }
        public override void DeserializeMember(Catel.Runtime.Serialization.ISerializationContext context, Catel.Runtime.Serialization.MemberValue memberValue) { }
        public override void SerializeMember(Catel.Runtime.Serialization.ISerializationContext context, Catel.Runtime.Serialization.MemberValue memberValue) { }
    }
    public class PropertyExpressionSerializerModifier : Catel.Runtime.Serialization.SerializerModifierBase<Orc.FilterBuilder.PropertyExpression>
    {
        public PropertyExpressionSerializerModifier() { }
        public override void DeserializeMember(Catel.Runtime.Serialization.ISerializationContext context, Catel.Runtime.Serialization.MemberValue memberValue) { }
        public override void SerializeMember(Catel.Runtime.Serialization.ISerializationContext context, Catel.Runtime.Serialization.MemberValue memberValue) { }
    }
}
namespace Orc.FilterBuilder.Services
{
    
    public class FilterCustomizationService : Orc.FilterBuilder.Services.IFilterCustomizationService
    {
        public FilterCustomizationService() { }
        public virtual void CustomizeInstanceProperties(Orc.FilterBuilder.Models.IPropertyCollection instanceProperties) { }
    }
    public class FilterSchemeManager : Orc.FilterBuilder.Services.IFilterSchemeManager
    {
        public FilterSchemeManager(Catel.Runtime.Serialization.Xml.IXmlSerializer xmlSerializer) { }
        public bool AutoSave { get; set; }
        public Orc.FilterBuilder.Models.FilterSchemes FilterSchemes { get; }
        public object Scope { get; set; }
        public event System.EventHandler<System.EventArgs> Loaded;
        public event System.EventHandler<System.EventArgs> Saved;
        public event System.EventHandler<System.EventArgs> Updated;
        public void Load(string fileName = null) { }
        public System.Threading.Tasks.Task<bool> LoadAsync(string fileName = null) { }
        public void Save(string fileName = null) { }
        public void UpdateFilters() { }
    }
    public class FilterService : Orc.FilterBuilder.Services.IFilterService
    {
        public FilterService(Orc.FilterBuilder.Services.IFilterSchemeManager filterSchemeManager) { }
        public Orc.FilterBuilder.Models.FilterScheme SelectedFilter { get; set; }
        public event System.EventHandler<System.EventArgs> FiltersUpdated;
        public event System.EventHandler<System.EventArgs> SelectedFilterChanged;
        [MethodTimer.TimeAttribute()]
        public void FilterCollection(Orc.FilterBuilder.Models.FilterScheme filter, System.Collections.IEnumerable rawCollection, System.Collections.IList filteredCollection) { }
        public System.Threading.Tasks.Task FilterCollectionAsync(Orc.FilterBuilder.Models.FilterScheme filter, System.Collections.IEnumerable rawCollection, System.Collections.IList filteredCollection) { }
    }
    public interface IFilterCustomizationService
    {
        void CustomizeInstanceProperties(Orc.FilterBuilder.Models.IPropertyCollection instanceProperties);
    }
    public interface IFilterSchemeManager
    {
        bool AutoSave { get; set; }
        Orc.FilterBuilder.Models.FilterSchemes FilterSchemes { get; }
        object Scope { get; set; }
        public event System.EventHandler<System.EventArgs> Loaded;
        public event System.EventHandler<System.EventArgs> Saved;
        public event System.EventHandler<System.EventArgs> Updated;
        void Load(string fileName = null);
        System.Threading.Tasks.Task<bool> LoadAsync(string fileName = null);
        void Save(string fileName = null);
        void UpdateFilters();
    }
    public interface IFilterService
    {
        Orc.FilterBuilder.Models.FilterScheme SelectedFilter { get; set; }
        public event System.EventHandler<System.EventArgs> FiltersUpdated;
        public event System.EventHandler<System.EventArgs> SelectedFilterChanged;
        void FilterCollection(Orc.FilterBuilder.Models.FilterScheme filter, System.Collections.IEnumerable rawCollection, System.Collections.IList filteredCollection);
        System.Threading.Tasks.Task FilterCollectionAsync(Orc.FilterBuilder.Models.FilterScheme filter, System.Collections.IEnumerable rawCollection, System.Collections.IList filteredCollection);
    }
    public class static IFilterServiceExtensions
    {
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TItem>> FilterCollectionAsync<TItem>(this Orc.FilterBuilder.Services.IFilterService filterService, Orc.FilterBuilder.Models.FilterScheme filter, System.Collections.Generic.IEnumerable<TItem> rawCollection) { }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TItem>> FilterCollectionWithCurrentFilterAsync<TItem>(this Orc.FilterBuilder.Services.IFilterService filterService, System.Collections.Generic.IEnumerable<TItem> rawCollection) { }
    }
    public interface IReflectionService
    {
        void ClearCache();
        Orc.FilterBuilder.Models.IPropertyCollection GetInstanceProperties(System.Type targetType);
    }
    public class ReflectionService : Orc.FilterBuilder.Services.IReflectionService
    {
        public ReflectionService(Orc.FilterBuilder.Services.IFilterCustomizationService filterCustomizationService) { }
        public void ClearCache() { }
        public Orc.FilterBuilder.Models.IPropertyCollection GetInstanceProperties(System.Type targetType) { }
    }
}
namespace Orc.FilterBuilder.ViewModels
{
    
    public class EditFilterViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData AllowLivePreviewProperty;
        public static readonly Catel.Data.PropertyData EnableAutoCompletionProperty;
        public static readonly Catel.Data.PropertyData EnableLivePreviewProperty;
        public static readonly Catel.Data.PropertyData FilterSchemeProperty;
        public static readonly Catel.Data.PropertyData FilterSchemeTitleProperty;
        public static readonly Catel.Data.PropertyData InstancePropertiesProperty;
        public static readonly Catel.Data.PropertyData PreviewItemsProperty;
        public static readonly Catel.Data.PropertyData RawCollectionProperty;
        public EditFilterViewModel(Orc.FilterBuilder.Models.FilterSchemeEditInfo filterSchemeEditInfo, Catel.Runtime.Serialization.Xml.IXmlSerializer xmlSerializer, Catel.Services.IMessageService messageService, Catel.IoC.IServiceLocator serviceLocator, Catel.Services.ILanguageService languageService) { }
        public Catel.MVVM.Command<Orc.FilterBuilder.ConditionGroup> AddExpressionCommand { get; }
        public Catel.MVVM.Command<Orc.FilterBuilder.ConditionGroup> AddGroupCommand { get; }
        public bool AllowLivePreview { get; }
        public Catel.MVVM.Command<Orc.FilterBuilder.ConditionTreeItem> DeleteConditionItem { get; }
        public bool EnableAutoCompletion { get; }
        public bool EnableLivePreview { get; set; }
        public Orc.FilterBuilder.Models.FilterScheme FilterScheme { get; }
        public string FilterSchemeTitle { get; set; }
        public System.Collections.Generic.List<Orc.FilterBuilder.Models.IPropertyMetadata> InstanceProperties { get; }
        public Catel.Collections.FastObservableCollection<object> PreviewItems { get; }
        public System.Collections.IEnumerable RawCollection { get; }
        public override string Title { get; }
        protected override System.Threading.Tasks.Task<bool> CancelAsync() { }
        protected override System.Threading.Tasks.Task CloseAsync() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
        protected override System.Threading.Tasks.Task<bool> SaveAsync() { }
        protected override void ValidateFields(System.Collections.Generic.List<Catel.Data.IFieldValidationResult> validationResults) { }
    }
    public class FilterBuilderViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData AllowDeleteProperty;
        public static readonly Catel.Data.PropertyData AllowLivePreviewProperty;
        public static readonly Catel.Data.PropertyData AllowResetProperty;
        public static readonly Catel.Data.PropertyData AutoApplyFilterProperty;
        public static readonly Catel.Data.PropertyData AvailableSchemesProperty;
        public static readonly Catel.Data.PropertyData EnableAutoCompletionProperty;
        public static readonly Catel.Data.PropertyData FilteredCollectionProperty;
        public static readonly Catel.Data.PropertyData FilteringFuncProperty;
        public static readonly Catel.Data.PropertyData ModeProperty;
        public static readonly Catel.Data.PropertyData RawCollectionProperty;
        public static readonly Catel.Data.PropertyData ScopeProperty;
        public static readonly Catel.Data.PropertyData SelectedFilterSchemeProperty;
        public FilterBuilderViewModel(Catel.Services.IUIVisualizerService uiVisualizerService, Orc.FilterBuilder.Services.IFilterSchemeManager filterSchemeManager, Orc.FilterBuilder.Services.IFilterService filterService, Catel.Services.IMessageService messageService, Catel.IoC.IServiceLocator serviceLocator, Orc.FilterBuilder.Services.IReflectionService reflectionService, Catel.Services.ILanguageService languageService) { }
        public bool AllowDelete { get; set; }
        public bool AllowLivePreview { get; set; }
        public bool AllowReset { get; set; }
        public Catel.MVVM.TaskCommand ApplySchemeCommand { get; }
        public bool AutoApplyFilter { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.FilterBuilder.Models.FilterScheme> AvailableSchemes { get; }
        public Catel.MVVM.Command<Orc.FilterBuilder.Models.FilterScheme> DeleteSchemeCommand { get; }
        public Catel.MVVM.TaskCommand<Orc.FilterBuilder.Models.FilterScheme> EditSchemeCommand { get; }
        public bool EnableAutoCompletion { get; set; }
        public System.Collections.IList FilteredCollection { get; set; }
        public System.Func<object, bool> FilteringFunc { get; set; }
        public Orc.FilterBuilder.FilterBuilderMode Mode { get; set; }
        public Catel.MVVM.TaskCommand NewSchemeCommand { get; }
        public System.Collections.IEnumerable RawCollection { get; set; }
        public Catel.MVVM.Command ResetSchemeCommand { get; }
        public object Scope { get; set; }
        public Orc.FilterBuilder.Models.FilterScheme SelectedFilterScheme { get; set; }
        protected override System.Threading.Tasks.Task CloseAsync() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
}
namespace Orc.FilterBuilder.Views
{
    
    public sealed class EditFilterView : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public EditFilterView() { }
        public void InitializeComponent() { }
        protected override void OnViewModelChanged() { }
    }
    public class EditFilterWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public EditFilterWindow() { }
        public void InitializeComponent() { }
    }
    public class FilterBuilderControl : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty AccentColorBrushProperty;
        public static readonly System.Windows.DependencyProperty AllowDeleteProperty;
        public static readonly System.Windows.DependencyProperty AllowLivePreviewProperty;
        public static readonly System.Windows.DependencyProperty AllowResetProperty;
        public static readonly System.Windows.DependencyProperty AutoApplyFilterProperty;
        public static readonly System.Windows.DependencyProperty EnableAutoCompletionProperty;
        public static readonly System.Windows.DependencyProperty FilteredCollectionProperty;
        public static readonly System.Windows.DependencyProperty FilteringFuncProperty;
        public static readonly System.Windows.DependencyProperty ModeProperty;
        public static readonly System.Windows.DependencyProperty RawCollectionProperty;
        public static readonly System.Windows.DependencyProperty ScopeProperty;
        public FilterBuilderControl() { }
        public System.Windows.Media.Brush AccentColorBrush { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public bool AllowDelete { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public bool AllowLivePreview { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public bool AllowReset { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public bool AutoApplyFilter { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public bool EnableAutoCompletion { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public System.Collections.IList FilteredCollection { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public System.Func<object, bool> FilteringFunc { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public Orc.FilterBuilder.FilterBuilderMode Mode { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public System.Collections.IEnumerable RawCollection { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public object Scope { get; set; }
        public void InitializeComponent() { }
        public override void OnApplyTemplate() { }
    }
}
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v3.1", FrameworkDisplayName="")]
public static class LoadAssembliesOnStartup { }
public static class ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.FilterBuilder
{
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class BooleanExpression : Orc.FilterBuilder.DataTypeExpression
    {
        public static readonly Catel.Data.PropertyData BooleanValuesProperty;
        public static readonly Catel.Data.PropertyData ValueProperty;
        public BooleanExpression() { }
        [Catel.Runtime.Serialization.ExcludeFromSerialization]
        public System.Collections.Generic.List<bool> BooleanValues { get; set; }
        public bool Value { get; set; }
        public override bool CalculateResult(Orc.FilterBuilder.IPropertyMetadata propertyMetadata, object entity) { }
        public override string ToString() { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class ByteExpression : Orc.FilterBuilder.NumericExpression<byte>
    {
        public ByteExpression() { }
        public ByteExpression(bool isNullable) { }
    }
    public static class CollectionHelper
    {
        public static System.Type GetTargetType(System.Collections.IEnumerable collection) { }
    }
    public enum Condition
    {
        [Catel.ComponentModel.DisplayName("FilterBuilder_Contains")]
        Contains = 0,
        [Catel.ComponentModel.DisplayName("FilterBuilder_StartsWith")]
        StartsWith = 1,
        [Catel.ComponentModel.DisplayName("FilterBuilder_EndsWith")]
        EndsWith = 2,
        [Catel.ComponentModel.DisplayName("FilterBuilder_EqualTo")]
        EqualTo = 3,
        [Catel.ComponentModel.DisplayName("FilterBuilder_NotEqualTo")]
        NotEqualTo = 4,
        [Catel.ComponentModel.DisplayName("FilterBuilder_GreaterThan")]
        GreaterThan = 5,
        [Catel.ComponentModel.DisplayName("FilterBuilder_LessThan")]
        LessThan = 6,
        [Catel.ComponentModel.DisplayName("FilterBuilder_GreaterThanOrEqualTo")]
        GreaterThanOrEqualTo = 7,
        [Catel.ComponentModel.DisplayName("FilterBuilder_LessThanOrEqualTo")]
        LessThanOrEqualTo = 8,
        [Catel.ComponentModel.DisplayName("FilterBuilder_IsEmpty")]
        IsEmpty = 9,
        [Catel.ComponentModel.DisplayName("FilterBuilder_NotIsEmpty")]
        NotIsEmpty = 10,
        [Catel.ComponentModel.DisplayName("FilterBuilder_IsNull")]
        IsNull = 11,
        [Catel.ComponentModel.DisplayName("FilterBuilder_NotIsNull")]
        NotIsNull = 12,
        [Catel.ComponentModel.DisplayName("FilterBuilder_Matches")]
        Matches = 13,
        [Catel.ComponentModel.DisplayName("FilterBuilder_DoesNotMatch")]
        DoesNotMatch = 14,
        [Catel.ComponentModel.DisplayName("FilterBuilder_DoesNotContain")]
        DoesNotContain = 15,
        [Catel.ComponentModel.DisplayName("FilterBuilder_DoesNotStartWith")]
        DoesNotStartWith = 16,
        [Catel.ComponentModel.DisplayName("FilterBuilder_DoesNotEndWith")]
        DoesNotEndWith = 17,
    }
    public static class ConditionExtensions
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
        [Catel.ComponentModel.DisplayName("FilterBuilder_And")]
        And = 0,
        [Catel.ComponentModel.DisplayName("FilterBuilder_Or")]
        Or = 1,
    }
    public static class ConditionHelper
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
        [Catel.Data.ExcludeFromValidation]
        [Catel.Runtime.Serialization.ExcludeFromSerialization]
        public bool IsValid { get; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.FilterBuilder.ConditionTreeItem> Items { get; }
        [Catel.Runtime.Serialization.ExcludeFromSerialization]
        public Orc.FilterBuilder.ConditionTreeItem Parent { get; set; }
        public event System.EventHandler<System.EventArgs> Updated;
        public abstract bool CalculateResult(object entity);
        protected bool Equals(Orc.FilterBuilder.ConditionTreeItem other) { }
        public override bool Equals(object obj) { }
        public override int GetHashCode() { }
        protected override void OnDeserialized() { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
        protected override void OnValidated(Catel.Data.IValidationContext validationContext) { }
        protected void RaiseUpdated() { }
    }
    public static class ConditionTreeItemExtensions
    {
        public static bool IsRoot(this Orc.FilterBuilder.ConditionTreeItem item) { }
    }
    public static class ConditionsLinqExtensions
    {
        public static System.Linq.Expressions.Expression<System.Func<T, bool>> BuildLambda<T>(this Orc.FilterBuilder.ConditionTreeItem conditionTreeItem) { }
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
        public abstract bool CalculateResult(Orc.FilterBuilder.IPropertyMetadata propertyMetadata, object entity);
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class DateTimeExpression : Orc.FilterBuilder.ValueDataTypeExpression<System.DateTime>
    {
        public DateTimeExpression() { }
        public DateTimeExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class DecimalExpression : Orc.FilterBuilder.NumericExpression<decimal>
    {
        public DecimalExpression() { }
        public DecimalExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class DoubleExpression : Orc.FilterBuilder.NumericExpression<double>
    {
        public DoubleExpression() { }
        public DoubleExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class EnumExpression<TEnum> : Orc.FilterBuilder.NullableDataTypeExpression
        where TEnum :  struct
    {
        public static readonly Catel.Data.PropertyData ValueProperty;
        public EnumExpression() { }
        public EnumExpression(bool isNullable) { }
        public System.Collections.Generic.List<TEnum> EnumValues { get; }
        public TEnum Value { get; set; }
        public override sealed bool CalculateResult(Orc.FilterBuilder.IPropertyMetadata propertyMetadata, object entity) { }
        public override string ToString() { }
    }
    public class FilterCustomizationService : Orc.FilterBuilder.IFilterCustomizationService
    {
        public FilterCustomizationService() { }
        public virtual void CustomizeInstanceProperties(Orc.FilterBuilder.IPropertyCollection instanceProperties) { }
    }
    [Catel.Runtime.Serialization.SerializerModifier(typeof(Orc.FilterBuilder.Runtime.Serialization.FilterSchemeSerializerModifier))]
    public class FilterScheme : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData CanDeleteProperty;
        public static readonly Catel.Data.PropertyData CanEditProperty;
        public static readonly Catel.Data.PropertyData ConditionItemsProperty;
        public static readonly Catel.Data.PropertyData FilterGroupProperty;
        public static readonly Catel.Data.PropertyData HasInvalidConditionItemsProperty;
        public static readonly Catel.Data.PropertyData TargetTypeProperty;
        public static readonly Catel.Data.PropertyData TitleProperty;
        public FilterScheme() { }
        public FilterScheme(System.Type targetType) { }
        public FilterScheme(System.Type targetType, string title) { }
        public FilterScheme(System.Type targetType, string title, Orc.FilterBuilder.ConditionTreeItem root) { }
        [Catel.Runtime.Serialization.ExcludeFromSerialization]
        public bool CanDelete { get; set; }
        [Catel.Runtime.Serialization.ExcludeFromSerialization]
        public bool CanEdit { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.FilterBuilder.ConditionTreeItem> ConditionItems { get; }
        public string FilterGroup { get; set; }
        public bool HasInvalidConditionItems { get; }
        [Catel.Runtime.Serialization.ExcludeFromSerialization]
        public Orc.FilterBuilder.ConditionTreeItem Root { get; }
        [Catel.Runtime.Serialization.ExcludeFromSerialization]
        public object Scope { get; set; }
        [Catel.Runtime.Serialization.IncludeInSerialization]
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
        public void Update(Orc.FilterBuilder.FilterScheme otherScheme) { }
    }
    public class FilterSchemeEditInfo
    {
        public FilterSchemeEditInfo(Orc.FilterBuilder.FilterScheme filterScheme, System.Collections.IEnumerable rawCollection, bool allowLivePreview, bool enableAutoCompletion) { }
        public bool AllowLivePreview { get; }
        public bool EnableAutoCompletion { get; }
        public Orc.FilterBuilder.FilterScheme FilterScheme { get; }
        public System.Collections.IEnumerable RawCollection { get; }
    }
    public static class FilterSchemeExtensions
    {
        public static void Apply(this Orc.FilterBuilder.FilterScheme filterScheme, System.Collections.IEnumerable rawCollection, System.Collections.IList filteredCollection) { }
        public static void EnsureIntegrity(this Orc.FilterBuilder.FilterScheme filterScheme, Orc.FilterBuilder.IReflectionService reflectionService) { }
    }
    public class FilterSchemeManager : Orc.FilterBuilder.IFilterSchemeManager
    {
        public FilterSchemeManager(Orc.FilterBuilder.IFilterSerializationService filterSerializationService, Catel.Services.IAppDataService appDataService) { }
        public bool AutoSave { get; set; }
        public Orc.FilterBuilder.FilterSchemes FilterSchemes { get; }
        public object Scope { get; set; }
        public event System.EventHandler<System.EventArgs> Loaded;
        public event System.EventHandler<System.EventArgs> Saved;
        public event System.EventHandler<System.EventArgs> Updated;
        protected virtual string GetDefaultFileName() { }
        public virtual System.Threading.Tasks.Task<bool> LoadAsync(string fileName = null) { }
        public virtual System.Threading.Tasks.Task SaveAsync(string fileName = null) { }
        public virtual System.Threading.Tasks.Task UpdateFiltersAsync() { }
    }
    public class FilterSchemes : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData SchemesProperty;
        public FilterSchemes() { }
        public System.Collections.ObjectModel.ObservableCollection<Orc.FilterBuilder.FilterScheme> Schemes { get; }
        [Catel.Runtime.Serialization.ExcludeFromSerialization]
        public object Scope { get; set; }
    }
    public class FilterSerializationService : Orc.FilterBuilder.IFilterSerializationService
    {
        public FilterSerializationService(Orc.FileSystem.IDirectoryService directoryService, Orc.FileSystem.IFileService fileService, Catel.Runtime.Serialization.Xml.IXmlSerializer xmlSerializer) { }
        public virtual System.Threading.Tasks.Task<Orc.FilterBuilder.FilterSchemes> LoadFiltersAsync(string fileName) { }
        public virtual System.Threading.Tasks.Task SaveFiltersAsync(string fileName, Orc.FilterBuilder.FilterSchemes filterSchemes) { }
    }
    public class FilterService : Orc.FilterBuilder.IFilterService
    {
        public FilterService(Orc.FilterBuilder.IFilterSchemeManager filterSchemeManager) { }
        public Orc.FilterBuilder.FilterScheme SelectedFilter { get; set; }
        public event System.EventHandler<System.EventArgs> FiltersUpdated;
        public event System.EventHandler<System.EventArgs> SelectedFilterChanged;
        public void FilterCollection(Orc.FilterBuilder.FilterScheme filter, System.Collections.IEnumerable rawCollection, System.Collections.IList filteredCollection) { }
        public System.Threading.Tasks.Task FilterCollectionAsync(Orc.FilterBuilder.FilterScheme filter, System.Collections.IEnumerable rawCollection, System.Collections.IList filteredCollection) { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class FloatExpression : Orc.FilterBuilder.NumericExpression<float>
    {
        public FloatExpression() { }
        public FloatExpression(bool isNullable) { }
    }
    public interface IFilterCustomizationService
    {
        void CustomizeInstanceProperties(Orc.FilterBuilder.IPropertyCollection instanceProperties);
    }
    public interface IFilterSchemeManager
    {
        bool AutoSave { get; set; }
        Orc.FilterBuilder.FilterSchemes FilterSchemes { get; }
        object Scope { get; set; }
        event System.EventHandler<System.EventArgs> Loaded;
        event System.EventHandler<System.EventArgs> Saved;
        event System.EventHandler<System.EventArgs> Updated;
        System.Threading.Tasks.Task<bool> LoadAsync(string fileName = null);
        System.Threading.Tasks.Task SaveAsync(string fileName = null);
        System.Threading.Tasks.Task UpdateFiltersAsync();
    }
    public interface IFilterSerializationService
    {
        System.Threading.Tasks.Task<Orc.FilterBuilder.FilterSchemes> LoadFiltersAsync(string fileName);
        System.Threading.Tasks.Task SaveFiltersAsync(string fileName, Orc.FilterBuilder.FilterSchemes filterSchemes);
    }
    public interface IFilterService
    {
        Orc.FilterBuilder.FilterScheme SelectedFilter { get; set; }
        event System.EventHandler<System.EventArgs> FiltersUpdated;
        event System.EventHandler<System.EventArgs> SelectedFilterChanged;
        void FilterCollection(Orc.FilterBuilder.FilterScheme filter, System.Collections.IEnumerable rawCollection, System.Collections.IList filteredCollection);
        System.Threading.Tasks.Task FilterCollectionAsync(Orc.FilterBuilder.FilterScheme filter, System.Collections.IEnumerable rawCollection, System.Collections.IList filteredCollection);
    }
    public static class IFilterServiceExtensions
    {
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TItem>> FilterCollectionAsync<TItem>(this Orc.FilterBuilder.IFilterService filterService, Orc.FilterBuilder.FilterScheme filter, System.Collections.Generic.IEnumerable<TItem> rawCollection) { }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TItem>> FilterCollectionWithCurrentFilterAsync<TItem>(this Orc.FilterBuilder.IFilterService filterService, System.Collections.Generic.IEnumerable<TItem> rawCollection) { }
    }
    public interface IPropertyCollection
    {
        System.Collections.Generic.List<Orc.FilterBuilder.IPropertyMetadata> Properties { get; }
        Orc.FilterBuilder.IPropertyMetadata GetProperty(string propertyName);
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
    public interface IReflectionService
    {
        void ClearCache();
        Orc.FilterBuilder.IPropertyCollection GetInstanceProperties(System.Type targetType);
    }
    public class InstanceProperties : Orc.FilterBuilder.IPropertyCollection
    {
        public InstanceProperties(System.Type type) { }
        public System.Collections.Generic.List<Orc.FilterBuilder.IPropertyMetadata> Properties { get; }
        public Orc.FilterBuilder.IPropertyMetadata GetProperty(string propertyName) { }
    }
    public static class InstancePropertyHelper
    {
        public static bool IsSupportedType(this Orc.FilterBuilder.IPropertyMetadata property) { }
        public static bool IsSupportedType(this System.Reflection.PropertyInfo property) { }
        public static bool IsSupportedType(this System.Type type) { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class IntegerExpression : Orc.FilterBuilder.NumericExpression<int>
    {
        public IntegerExpression() { }
        public IntegerExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
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
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class NumericExpression : Orc.FilterBuilder.NumericExpression<double>
    {
        public NumericExpression() { }
        public NumericExpression(System.Type type) { }
    }
    public abstract class NumericExpression<TValue> : Orc.FilterBuilder.ValueDataTypeExpression<TValue>
        where TValue :  struct, System.IComparable, System.IFormattable, System.IConvertible, System.IComparable<TValue>, System.IEquatable<TValue>
    {
        public static readonly Catel.Data.PropertyData IsDecimalProperty;
        public static readonly Catel.Data.PropertyData IsSignedProperty;
        protected NumericExpression() { }
        public bool IsDecimal { get; set; }
        public bool IsSigned { get; set; }
    }
    [Catel.Data.ValidateModel(typeof(Orc.FilterBuilder.PropertyExpressionValidator))]
    [Catel.Runtime.Serialization.SerializerModifier(typeof(Orc.FilterBuilder.Runtime.Serialization.PropertyExpressionSerializerModifier))]
    [System.Diagnostics.DebuggerDisplay("{Property} = {DataTypeExpression}")]
    public class PropertyExpression : Orc.FilterBuilder.ConditionTreeItem
    {
        public static readonly Catel.Data.PropertyData DataTypeExpressionProperty;
        public static readonly Catel.Data.PropertyData PropertyProperty;
        public static readonly Catel.Data.PropertyData PropertySerializationValueProperty;
        public PropertyExpression() { }
        public Orc.FilterBuilder.DataTypeExpression DataTypeExpression { get; set; }
        public Orc.FilterBuilder.IPropertyMetadata Property { get; set; }
        public override bool CalculateResult(object entity) { }
        protected override void OnDeserialized() { }
        public override string ToString() { }
    }
    public class PropertyExpressionValidator : Catel.Data.ValidatorBase<Orc.FilterBuilder.PropertyExpression>
    {
        public PropertyExpressionValidator() { }
        protected override void ValidateFields(Orc.FilterBuilder.PropertyExpression instance, System.Collections.Generic.List<Catel.Data.IFieldValidationResult> validationResults) { }
    }
    [System.Diagnostics.DebuggerDisplay("{OwnerType}.{Name}")]
    public class PropertyMetadata : Orc.FilterBuilder.IPropertyMetadata
    {
        public PropertyMetadata(System.Type ownerType, Catel.Data.PropertyData propertyData) { }
        public PropertyMetadata(System.Type ownerType, System.Reflection.PropertyInfo propertyInfo) { }
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
    public class ReflectionService : Orc.FilterBuilder.IReflectionService
    {
        public ReflectionService(Orc.FilterBuilder.IFilterCustomizationService filterCustomizationService) { }
        public void ClearCache() { }
        public Orc.FilterBuilder.IPropertyCollection GetInstanceProperties(System.Type targetType) { }
    }
    public static class RegexHelper
    {
        public static bool IsValid(string pattern) { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class SByteExpression : Orc.FilterBuilder.NumericExpression<sbyte>
    {
        public SByteExpression() { }
        public SByteExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class ShortExpression : Orc.FilterBuilder.NumericExpression<short>
    {
        public ShortExpression() { }
        public ShortExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class StringExpression : Orc.FilterBuilder.DataTypeExpression
    {
        public static readonly Catel.Data.PropertyData ValueProperty;
        public StringExpression() { }
        public string Value { get; set; }
        public override bool CalculateResult(Orc.FilterBuilder.IPropertyMetadata propertyMetadata, object entity) { }
        public override string ToString() { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
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
        [Catel.Runtime.Serialization.ExcludeFromSerialization]
        public System.Collections.Generic.List<Orc.FilterBuilder.TimeSpanType> SpanTypes { get; set; }
        public System.TimeSpan Value { get; set; }
        public override bool CalculateResult(Orc.FilterBuilder.IPropertyMetadata propertyMetadata, object entity) { }
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
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedIntegerExpression : Orc.FilterBuilder.NumericExpression<uint>
    {
        public UnsignedIntegerExpression() { }
        public UnsignedIntegerExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedLongExpression : Orc.FilterBuilder.NumericExpression<ulong>
    {
        public UnsignedLongExpression() { }
        public UnsignedLongExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
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
        where TValue :  struct, System.IComparable, System.IFormattable, System.IConvertible, System.IComparable<TValue>, System.IEquatable<TValue>
    {
        public static readonly Catel.Data.PropertyData ValueProperty;
        protected ValueDataTypeExpression() { }
        public TValue Value { get; set; }
        public override bool CalculateResult(Orc.FilterBuilder.IPropertyMetadata propertyMetadata, object entity) { }
        public override string ToString() { }
    }
}
namespace Orc.FilterBuilder.Runtime.Serialization
{
    public class FilterSchemeSerializerModifier : Catel.Runtime.Serialization.SerializerModifierBase<Orc.FilterBuilder.FilterScheme>
    {
        public FilterSchemeSerializerModifier() { }
        public override void DeserializeMember(Catel.Runtime.Serialization.ISerializationContext context, Catel.Runtime.Serialization.MemberValue memberValue) { }
        public override void SerializeMember(Catel.Runtime.Serialization.ISerializationContext context, Catel.Runtime.Serialization.MemberValue memberValue) { }
    }
    public class PropertyExpressionSerializerModifier : Catel.Runtime.Serialization.SerializerModifierBase<Orc.FilterBuilder.PropertyExpression>
    {
        public PropertyExpressionSerializerModifier(Orc.FilterBuilder.IReflectionService reflectionService) { }
        public override void DeserializeMember(Catel.Runtime.Serialization.ISerializationContext context, Catel.Runtime.Serialization.MemberValue memberValue) { }
        public override void SerializeMember(Catel.Runtime.Serialization.ISerializationContext context, Catel.Runtime.Serialization.MemberValue memberValue) { }
    }
}
[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.6", FrameworkDisplayName=".NET Framework 4.6")]
public class static ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.FilterBuilder
{
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class BooleanExpression : Orc.FilterBuilder.DataTypeExpression
    {
        public static readonly Catel.Data.PropertyData BooleanValuesProperty;
        public static readonly Catel.Data.PropertyData ValueProperty;
        protected BooleanExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
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
        protected ByteExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
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
        protected ConditionGroup(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
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
        protected ConditionTreeItem(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        protected ConditionTreeItem() { }
        [Catel.Data.ExcludeFromValidationAttribute()]
        [Catel.Runtime.Serialization.ExcludeFromSerializationAttribute()]
        public bool IsValid { get; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.FilterBuilder.ConditionTreeItem> Items { get; }
        [Catel.Runtime.Serialization.ExcludeFromSerializationAttribute()]
        public Orc.FilterBuilder.ConditionTreeItem Parent { get; set; }
        public event System.EventHandler<System.EventArgs> Updated;
        public abstract bool CalculateResult(object entity);
        protected bool Equals(Orc.FilterBuilder.ConditionTreeItem other) { }
        public override bool Equals(object obj) { }
        public override int GetHashCode() { }
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
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
        protected DataTypeExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        protected DataTypeExpression() { }
        public bool IsValueRequired { get; set; }
        public Orc.FilterBuilder.Condition SelectedCondition { get; set; }
        public Orc.FilterBuilder.ValueControlType ValueControlType { get; set; }
        public abstract bool CalculateResult(Orc.FilterBuilder.Models.IPropertyMetadata propertyMetadata, object entity);
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class DateTimeExpression : Orc.FilterBuilder.ValueDataTypeExpression<System.DateTime>
    {
        protected DateTimeExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public DateTimeExpression() { }
        public DateTimeExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class DecimalExpression : Orc.FilterBuilder.NumericExpression<decimal>
    {
        protected DecimalExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public DecimalExpression() { }
        public DecimalExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class DoubleExpression : Orc.FilterBuilder.NumericExpression<double>
    {
        protected DoubleExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public DoubleExpression() { }
        public DoubleExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class EnumExpression<TEnum> : Orc.FilterBuilder.NullableDataTypeExpression
        where TEnum :  struct
    {
        public static readonly Catel.Data.PropertyData ValueProperty;
        protected EnumExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public EnumExpression(bool isNullable) { }
        public System.Collections.Generic.List<TEnum> EnumValues { get; }
        public TEnum Value { get; set; }
        public virtual bool CalculateResult(Orc.FilterBuilder.Models.IPropertyMetadata propertyMetadata, object entity) { }
        public override string ToString() { }
    }
    public class static FilterSchemeExtensions
    {
        public static void Apply(this Orc.FilterBuilder.Models.FilterScheme filterScheme, System.Collections.IEnumerable rawCollection, System.Collections.IList filteredCollection) { }
        public static void EnsureIntegrity(this Orc.FilterBuilder.Models.FilterScheme filterScheme, Orc.FilterBuilder.Services.IReflectionService reflectionService) { }
    }
    public class FilterSerializationService : Orc.FilterBuilder.IFilterSerializationService
    {
        public FilterSerializationService(Orc.FileSystem.IDirectoryService directoryService, Orc.FileSystem.IFileService fileService, Catel.Runtime.Serialization.Xml.IXmlSerializer xmlSerializer) { }
        public virtual System.Threading.Tasks.Task<Orc.FilterBuilder.Models.FilterSchemes> LoadFiltersAsync(string fileName) { }
        public virtual System.Threading.Tasks.Task SaveFiltersAsync(string fileName, Orc.FilterBuilder.Models.FilterSchemes filterSchemes) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class FloatExpression : Orc.FilterBuilder.NumericExpression<float>
    {
        protected FloatExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public FloatExpression() { }
        public FloatExpression(bool isNullable) { }
    }
    public interface IFilterSerializationService
    {
        System.Threading.Tasks.Task<Orc.FilterBuilder.Models.FilterSchemes> LoadFiltersAsync(string fileName);
        System.Threading.Tasks.Task SaveFiltersAsync(string fileName, Orc.FilterBuilder.Models.FilterSchemes filterSchemes);
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
        protected IntegerExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public IntegerExpression() { }
        public IntegerExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class LongExpression : Orc.FilterBuilder.NumericExpression<long>
    {
        protected LongExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public LongExpression() { }
        public LongExpression(bool isNullable) { }
    }
    public abstract class NullableDataTypeExpression : Orc.FilterBuilder.DataTypeExpression
    {
        public static readonly Catel.Data.PropertyData IsNullableProperty;
        protected NullableDataTypeExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        protected NullableDataTypeExpression() { }
        public bool IsNullable { get; set; }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class NumericExpression : Orc.FilterBuilder.NumericExpression<double>
    {
        protected NumericExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public NumericExpression() { }
        public NumericExpression(System.Type type) { }
    }
    public abstract class NumericExpression<TValue> : Orc.FilterBuilder.ValueDataTypeExpression<TValue>
        where TValue :  struct, System.IComparable, System.IFormattable, System.IConvertible, System.IComparable<>, System.IEquatable<>
    {
        public static readonly Catel.Data.PropertyData IsDecimalProperty;
        public static readonly Catel.Data.PropertyData IsSignedProperty;
        protected NumericExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
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
        protected PropertyExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public Orc.FilterBuilder.DataTypeExpression DataTypeExpression { get; set; }
        public Orc.FilterBuilder.Models.IPropertyMetadata Property { get; set; }
        public override bool CalculateResult(object entity) { }
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
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
        protected SByteExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public SByteExpression() { }
        public SByteExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class ShortExpression : Orc.FilterBuilder.NumericExpression<short>
    {
        protected ShortExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public ShortExpression() { }
        public ShortExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class StringExpression : Orc.FilterBuilder.DataTypeExpression
    {
        public static readonly Catel.Data.PropertyData ValueProperty;
        protected StringExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
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
        protected TimeSpanExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
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
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedIntegerExpression : Orc.FilterBuilder.NumericExpression<uint>
    {
        protected UnsignedIntegerExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public UnsignedIntegerExpression() { }
        public UnsignedIntegerExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedLongExpression : Orc.FilterBuilder.NumericExpression<ulong>
    {
        protected UnsignedLongExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public UnsignedLongExpression() { }
        public UnsignedLongExpression(bool isNullable) { }
    }
    [System.Diagnostics.DebuggerDisplayAttribute("{ValueControlType} {SelectedCondition} {Value}")]
    public class UnsignedShortExpression : Orc.FilterBuilder.NumericExpression<ushort>
    {
        protected UnsignedShortExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
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
        protected ValueDataTypeExpression(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
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
namespace Orc.FilterBuilder.Models
{
    [Catel.Runtime.Serialization.SerializerModifierAttribute(typeof(Orc.FilterBuilder.Runtime.Serialization.FilterSchemeSerializerModifier))]
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
        protected FilterScheme(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
        public FilterScheme(System.Type targetType, string title, Orc.FilterBuilder.ConditionTreeItem root) { }
        [Catel.Runtime.Serialization.ExcludeFromSerializationAttribute()]
        public bool CanDelete { get; set; }
        [Catel.Runtime.Serialization.ExcludeFromSerializationAttribute()]
        public bool CanEdit { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.FilterBuilder.ConditionTreeItem> ConditionItems { get; }
        public string FilterGroup { get; set; }
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
        protected FilterSchemes(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
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
        public PropertyExpressionSerializerModifier(Orc.FilterBuilder.Services.IReflectionService reflectionService) { }
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
        public FilterSchemeManager(Orc.FilterBuilder.IFilterSerializationService filterSerializationService, Catel.Services.IAppDataService appDataService) { }
        public bool AutoSave { get; set; }
        public Orc.FilterBuilder.Models.FilterSchemes FilterSchemes { get; }
        public object Scope { get; set; }
        public event System.EventHandler<System.EventArgs> Loaded;
        public event System.EventHandler<System.EventArgs> Saved;
        public event System.EventHandler<System.EventArgs> Updated;
        protected virtual string GetDefaultFileName() { }
        [System.ObsoleteAttribute("Use `IFilterSerializationService.LoadFiltersAsync` instead. Will be removed in ve" +
            "rsion 4.0.0.", true)]
        public void Load(string fileName = null) { }
        public virtual System.Threading.Tasks.Task<bool> LoadAsync(string fileName = null) { }
        [System.ObsoleteAttribute("Use `IFilterSerializationService.SaveFiltersAsync` instead. Will be removed in ve" +
            "rsion 4.0.0.", true)]
        public void Save(string fileName = null) { }
        public virtual System.Threading.Tasks.Task SaveAsync(string fileName = null) { }
        [System.ObsoleteAttribute("Use `UpdateFiltersAsync` instead. Will be removed in version 4.0.0.", true)]
        public void UpdateFilters() { }
        public virtual System.Threading.Tasks.Task UpdateFiltersAsync() { }
    }
    public class FilterService : Orc.FilterBuilder.Services.IFilterService
    {
        public FilterService(Orc.FilterBuilder.Services.IFilterSchemeManager filterSchemeManager) { }
        public Orc.FilterBuilder.Models.FilterScheme SelectedFilter { get; set; }
        public event System.EventHandler<System.EventArgs> FiltersUpdated;
        public event System.EventHandler<System.EventArgs> SelectedFilterChanged;
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
        [System.ObsoleteAttribute("Use `IFilterSerializationService.LoadFiltersAsync` instead. Will be removed in ve" +
            "rsion 4.0.0.", true)]
        void Load(string fileName = null);
        System.Threading.Tasks.Task<bool> LoadAsync(string fileName = null);
        [System.ObsoleteAttribute("Use `IFilterSerializationService.SaveFiltersAsync` instead. Will be removed in ve" +
            "rsion 4.0.0.", true)]
        void Save(string fileName = null);
        System.Threading.Tasks.Task SaveAsync(string fileName = null);
        [System.ObsoleteAttribute("Use `UpdateFiltersAsync` instead. Will be removed in version 4.0.0.", true)]
        void UpdateFilters();
        System.Threading.Tasks.Task UpdateFiltersAsync();
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
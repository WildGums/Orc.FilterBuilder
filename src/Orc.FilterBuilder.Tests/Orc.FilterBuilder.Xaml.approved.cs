[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.6", FrameworkDisplayName=".NET Framework 4.6")]
[assembly: System.Windows.Markup.XmlnsDefinitionAttribute("http://schemas.wildgums.com/orc/filterbuilder", "Orc.FilterBuilder")]
[assembly: System.Windows.Markup.XmlnsDefinitionAttribute("http://schemas.wildgums.com/orc/filterbuilder", "Orc.FilterBuilder.Behaviors")]
[assembly: System.Windows.Markup.XmlnsDefinitionAttribute("http://schemas.wildgums.com/orc/filterbuilder", "Orc.FilterBuilder.Converters")]
[assembly: System.Windows.Markup.XmlnsDefinitionAttribute("http://schemas.wildgums.com/orc/filterbuilder", "Orc.FilterBuilder.Views")]
[assembly: System.Windows.Markup.XmlnsPrefixAttribute("http://schemas.wildgums.com/orc/filterbuilder", "orcfilterbuilder")]
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
namespace Orc.FilterBuilder
{
    public enum FilterBuilderMode
    {
        Collection = 0,
        FilteringFunction = 1,
    }
    public class static TreeViewItemExtensions
    {
        public static int GetDepth(this System.Windows.Controls.TreeViewItem item) { }
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
        public Catel.MVVM.TaskCommand<Orc.FilterBuilder.Models.FilterScheme> DeleteSchemeCommand { get; }
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
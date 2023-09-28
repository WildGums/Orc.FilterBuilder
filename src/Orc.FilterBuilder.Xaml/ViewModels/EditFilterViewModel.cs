namespace Orc.FilterBuilder.ViewModels;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Runtime.Serialization.Xml;
using Catel.Services;

public class EditFilterViewModel : ViewModelBase
{
    private readonly FilterScheme _originalFilterScheme;
    private readonly IReflectionService _reflectionService;
    private readonly IXmlSerializer _xmlSerializer;
    private readonly IMessageService _messageService;
    private readonly ILanguageService _languageService;

    private readonly DispatcherTimer _applyFilterTimer;

    private bool _isFilterDirty;

    public EditFilterViewModel(FilterSchemeEditInfo filterSchemeEditInfo, IXmlSerializer xmlSerializer,
        IMessageService messageService, IServiceLocator serviceLocator, ILanguageService languageService)
    {
        ArgumentNullException.ThrowIfNull(filterSchemeEditInfo);
        ArgumentNullException.ThrowIfNull(xmlSerializer);
        ArgumentNullException.ThrowIfNull(messageService);
        ArgumentNullException.ThrowIfNull(serviceLocator);
        ArgumentNullException.ThrowIfNull(languageService);

        _xmlSerializer = xmlSerializer;
        _messageService = messageService;
        _languageService = languageService;

        DeferValidationUntilFirstSaveCall = true;

        PreviewItems = new FastObservableCollection<object>();
        RawCollection = filterSchemeEditInfo.RawCollection;
        EnableAutoCompletion = filterSchemeEditInfo.EnableAutoCompletion;
        AllowLivePreview = filterSchemeEditInfo.AllowLivePreview;

        var filterScheme = filterSchemeEditInfo.FilterScheme;
        _originalFilterScheme = filterScheme;

        _reflectionService = serviceLocator.ResolveRequiredType<IReflectionService>(filterScheme.Scope);
        InstanceProperties = _reflectionService.GetInstanceProperties(_originalFilterScheme.TargetType).Properties.ToList();

        FilterScheme = new FilterScheme(_originalFilterScheme.TargetType)
        {
            Scope = _originalFilterScheme.Scope
        };

        FilterSchemeTitle = string.Empty;

        AddGroupCommand = new Command<ConditionGroup>(OnAddGroup);
        AddExpressionCommand = new Command<ConditionGroup>(OnAddExpression);
        DeleteConditionItem = new Command<ConditionTreeItem>(OnDeleteCondition, OnDeleteConditionCanExecute);
        TogglePreview = new Command(OnTogglePreview);

        _applyFilterTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(200)
        };
        _applyFilterTimer.Tick += OnApplyFilterTimerTick;
    }

    public override string Title
    {
        get { return "Filter scheme"; }
    }

    public string FilterSchemeTitle { get; set; }
    public FilterScheme FilterScheme { get; }
    public bool EnableAutoCompletion { get; }
    public bool AllowLivePreview { get; }
    public bool EnableLivePreview { get; set; }
    public bool IsLivePreviewDirty { get; private set; }
    public IEnumerable RawCollection { get; }
    public bool IsPreviewVisible { get; set; }
    public FastObservableCollection<object> PreviewItems { get; }

    public List<IPropertyMetadata> InstanceProperties { get; private set; }

    public Command<ConditionGroup> AddGroupCommand { get; }
    public Command<ConditionGroup> AddExpressionCommand { get; }
    public Command<ConditionTreeItem> DeleteConditionItem { get; }
    public Command TogglePreview { get; }

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await using (var memoryStream = new MemoryStream())
        {
            _xmlSerializer.Serialize(_originalFilterScheme, memoryStream);
            memoryStream.Position = 0L;
            _xmlSerializer.Deserialize(FilterScheme, memoryStream);
        }

        FilterScheme.EnsureIntegrity(_reflectionService);
        FilterScheme.Scope = _originalFilterScheme.Scope;
        FilterSchemeTitle = FilterScheme.Title;

        RaisePropertyChanged(nameof(FilterScheme));

        UpdatePreviewItems();

        FilterScheme.Updated += OnFilterSchemeUpdated;
    }

    protected override async Task CloseAsync()
    {
        FilterScheme.Updated -= OnFilterSchemeUpdated;

        await base.CloseAsync();
    }

    private void OnFilterSchemeUpdated(object? sender, EventArgs e)
    {
        _isFilterDirty = true;

        UpdatePreviewItems();
    }

    protected override void ValidateFields(List<IFieldValidationResult> validationResults)
    {
        if (string.IsNullOrEmpty(FilterSchemeTitle))
        {
            validationResults.Add(FieldValidationResult.CreateError("FilterSchemeTitle", _languageService.GetRequiredString("FilterBuilder_FieldIsRequired")));
        }

        base.ValidateFields(validationResults);
    }

    protected override async Task<bool> CancelAsync()
    {
        if (_isFilterDirty && await _messageService.ShowAsync(_languageService.GetRequiredString("FilterBuilder_DiscardChanges"),
                _languageService.GetRequiredString("FilterBuilder_AreYouSure"), MessageButton.YesNo) == MessageResult.No)
        {
            return false;
        }

        return await base.CancelAsync();
    }

    protected override async Task<bool> SaveAsync()
    {
        if (FilterScheme.HasInvalidConditionItems && await _messageService.ShowAsync(_languageService.GetRequiredString("FilterBuilder_SaveBroken"),
                _languageService.GetRequiredString("FilterBuilder_AreYouSure"), MessageButton.YesNo) == MessageResult.No)
        {
            return false;
        }

        FilterScheme.Title = FilterSchemeTitle;
        _originalFilterScheme.Update(FilterScheme);

        return true;
    }

    private void OnTogglePreview()
    {
        IsPreviewVisible = !IsPreviewVisible;
    }

    private bool OnDeleteConditionCanExecute(ConditionTreeItem? item)
    {
        if (item is null)
        {
            return false;
        }

        if (!item.IsRoot())
        {
            return true;
        }

        return FilterScheme.ConditionItems.Count > 1;
    }

    private void OnDeleteCondition(ConditionTreeItem? item)
    {
        if (item is null)
        {
            return;
        }

        if (item.Parent is null)
        {
            FilterScheme.ConditionItems.Remove(item);

            foreach (var conditionItem in FilterScheme.ConditionItems)
            {
                conditionItem.Items.Remove(item);
            }
        }
        else
        {
            item.Parent.Items.Remove(item);
        }

        _isFilterDirty = true;

        UpdatePreviewItems();
    }

    private void OnAddExpression(ConditionGroup? group)
    {
        if (group is null)
        {
            return;
        }

        var propertyExpression = new PropertyExpression
        {
            Property = InstanceProperties.FirstOrDefault()
        };

        group.Items.Add(propertyExpression);
        propertyExpression.Parent = group;
    }

    private void OnAddGroup(ConditionGroup? group)
    {
        if (group is null)
        {
            return;
        }

        var newGroup = new ConditionGroup();
        group.Items.Add(newGroup);
        newGroup.Parent = group;
    }

    private void OnEnableLivePreviewChanged()
    {
        if (!EnableLivePreview)
        {
            IsPreviewVisible = false;
        }

        UpdatePreviewItems();
    }

    private void UpdatePreviewItems(bool forceUpdate = false)
    {
        if (FilterScheme is null || RawCollection is null)
        {
            return;
        }

        if (!AllowLivePreview)
        {
            return;
        }

        if (EnableLivePreview)
        {
            _applyFilterTimer.Stop();
            _applyFilterTimer.Start();
        }
        else
        {
            if (forceUpdate)
            {
                ApplyFilterScheme();
            }
            else
            {
                IsLivePreviewDirty = true;
            }
        }
    }

    private void ApplyFilterScheme()
    {
        FilterScheme?.Apply(RawCollection, PreviewItems);

        IsLivePreviewDirty = false;
    }

    private void OnApplyFilterTimerTick(object? sender, EventArgs e)
    {
        _applyFilterTimer.Stop();

        ApplyFilterScheme();
    }
}

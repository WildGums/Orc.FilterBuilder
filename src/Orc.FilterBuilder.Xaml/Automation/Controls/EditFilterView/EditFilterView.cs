namespace Orc.FilterBuilder.Automation;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Catel.Collections;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Views.EditFilterView))]
public class EditFilterView : FrameworkElement<EditFilterViewModel, EditFilterViewMap>,
    IEnumerable<EditFilterConditionTreeItemBase>
{
    public EditFilterView(AutomationElement element) 
        : base(element)
    {
    }

    public string Title
    {
        get => Map.FilterSchemeTitleEdit?.Text ?? string.Empty;
        set
        {
            var filterSchemeTitleEdit = Map.FilterSchemeTitleEdit;
            if (filterSchemeTitleEdit is null)
            {
                return;
            }

            filterSchemeTitleEdit.Text = value;
        }
    }

    public bool IsLivePreviewEnabled
    {
        get => Map.LivePreviewCheckBox?.IsChecked ?? false;
        set
        {
            var livePreviewCheckBox = Map.LivePreviewCheckBox;
            if (livePreviewCheckBox is null)
            {
                return;
            }

            livePreviewCheckBox.IsChecked = value;
        }
    }

    public bool IsPreviewCollectionVisible
    {
        get => Map.PreviewDataGrid?.IsVisible() ?? false;
        set
        {
            var isPreviewCollectionVisible = IsPreviewCollectionVisible;

            if (value && !isPreviewCollectionVisible
                || !value && isPreviewCollectionVisible)
            {
                Map.TogglePreviewLinkLabel?.Invoke();
            }
        }
    }

    public IReadOnlyList<object>? PreviewCollection
    {
        get => Map.PreviewDataGrid?.Current.ItemsSource?.OfType<object>().ToList();
    }

    public EditFilterConditionTreeItemBase? Root => Map.ConditionTree?.Children?.FirstOrDefault();
        
    public void Clear()
    {
        Map.ConditionTree?.Children?.ForEach(x => x.Delete());
    }

    public IEnumerator<EditFilterConditionTreeItemBase> GetEnumerator()
    {
#pragma warning disable IDISP001 // Dispose created
        var enumerator = Map.ConditionTree?.GetEnumerator();
#pragma warning restore IDISP001 // Dispose created
        if (enumerator is not null)
        {
            return enumerator;
        }

        return Enumerable.Empty<EditFilterConditionTreeItemBase>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
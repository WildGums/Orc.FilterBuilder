namespace Orc.FilterBuilder.Automation
{
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
            get => Map.FilterSchemeTitleEdit.Text;
            set => Map.FilterSchemeTitleEdit.Text = value;
        }

        public bool IsLivePreviewEnabled
        {
            get => Map.LivePreviewCheckBox.IsChecked == true;
            set => Map.LivePreviewCheckBox.IsChecked = value;
        }

        public bool IsPreviewCollectionVisible
        {
            get => Map.PreviewDataGrid.IsVisible();
            set
            {
                var isPreviewCollectionVisible = IsPreviewCollectionVisible;

                if (value && !isPreviewCollectionVisible
                    || !value && isPreviewCollectionVisible)
                {
                    Map.TogglePreviewLinkLabel.Invoke();
                }
            }
        }

        public IReadOnlyList<object> PreviewCollection
        {
            get => Map.PreviewDataGrid.Rows.Select(x => x.Current.DataContext).ToList();
        }

        public EditFilterConditionTreeItemBase Root => Map.ConditionTree?.Children?.FirstOrDefault();

        public void Clear()
        {
            Map.ConditionTree?.Children?.ForEach(x => x.Delete());
        }

        public IEnumerator<EditFilterConditionTreeItemBase> GetEnumerator()
        {
            return Map.ConditionTree.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

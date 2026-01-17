namespace Orc.FilterBuilder;

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using Catel.Data;

public class FilterScheme : ModelBase
{
    private static readonly Type DefaultTargetType = typeof(object);

    public FilterScheme()
        : this(DefaultTargetType)
    {
    }

    public FilterScheme(Type targetType)
        : this(targetType, string.Empty)
    {
    }

    public FilterScheme(Type targetType, string title)
        : this(targetType, title, new ConditionGroup())
    {
    }

    public FilterScheme(Type targetType, string title, ConditionTreeItem root)
    {
        ArgumentNullException.ThrowIfNull(targetType);
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(root);

        TargetType = targetType;
        Title = title;

        ConditionItems = [root];

        CanEdit = true;
        CanDelete = true;
    }

    public Type TargetType { get; private set; }

    public string Title { get; set; }

    public string? FilterGroup { get; set; }

    [JsonIgnore]
    public bool CanEdit { get; set; }

    [JsonIgnore]
    public bool CanDelete { get; set; }

    [JsonIgnore]
    public ConditionTreeItem Root
    {
        get { return ConditionItems.First(); }
    }

    public bool HasInvalidConditionItems { get; private set; }

    public ObservableCollection<ConditionTreeItem> ConditionItems { get; init; }

    public event EventHandler<EventArgs>? Updated;

    private void OnConditionItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (sender is not IList senderList)
        {
            return;
        }

        if (e.OldItems is not null)
        {
            foreach (var item in e.OldItems)
            {
                ((ConditionTreeItem)item).Updated -= OnConditionUpdated;
            }
        }

        var newCollection = e.Action == NotifyCollectionChangedAction.Reset 
            ? senderList
            : e.NewItems;
        if (newCollection is null)
        {
            return;
        }

        foreach (var item in newCollection)
        {
            ((ConditionTreeItem)item).Updated += OnConditionUpdated;
        }
    }

    private void OnConditionItemsChanged()
    {
        SubscribeToEvents();
    }

    private void CheckForInvalidItems()
    {
        HasInvalidConditionItems = (ConditionItems.Count > 0 && CountInvalidItems(Root) > 0);
    }

    private int CountInvalidItems(ConditionTreeItem conditionTreeItem)
    {
        ArgumentNullException.ThrowIfNull(conditionTreeItem);

        var items = conditionTreeItem.Items;
        if (items.Count == 0)
        {
            return conditionTreeItem.IsValid ? 0 : 1;
        }

        var invalidCount = items.Sum(CountInvalidItems);

        invalidCount += conditionTreeItem.IsValid ? 0 : 1;

        return invalidCount;
    }

    private void SubscribeToEvents()
    {
        var items = ConditionItems;
        items.CollectionChanged += OnConditionItemsCollectionChanged;

        foreach (var item in items)
        {
            item.Updated += OnConditionUpdated;
        }
    }

    private void OnConditionUpdated(object? sender, EventArgs e)
    {
        CheckForInvalidItems();

        RaiseUpdated();
    }

    public bool CalculateResult(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var root = Root;
        return root.CalculateResult(entity);
    }

    public void Update(FilterScheme otherScheme)
    {
        ArgumentNullException.ThrowIfNull(otherScheme);

        Title = otherScheme.Title;
        ConditionItems.Clear();
        ConditionItems.Add(otherScheme.Root);

        CheckForInvalidItems();

        RaiseUpdated();
    }

    protected void RaiseUpdated()
    {
        Updated?.Invoke(this, EventArgs.Empty);
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(Title);

        var rootString = Root.ToString();
        if (!string.IsNullOrEmpty(rootString))
        {
            if (rootString.StartsWith("((") && rootString.EndsWith("))"))
            {
                rootString = rootString[1..^1];
            }
        }

        if (string.IsNullOrEmpty(rootString))
        {
            return stringBuilder.ToString();
        }

        stringBuilder.AppendLine();
        stringBuilder.Append(rootString);

        return stringBuilder.ToString();
    }

    public override bool Equals(object? obj)
    {
        return obj is FilterScheme filterScheme 
               && string.Equals(filterScheme.Title, Title);
    }

    public override int GetHashCode()
    {
        return Title.GetHashCode();
    }
}

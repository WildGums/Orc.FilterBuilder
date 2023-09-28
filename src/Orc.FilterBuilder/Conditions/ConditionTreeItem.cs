namespace Orc.FilterBuilder;

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Catel.Data;
using Catel.Runtime.Serialization;

public abstract class ConditionTreeItem : ValidatableModelBase
{
    protected ConditionTreeItem()
    {
        Items = new ObservableCollection<ConditionTreeItem>();
    }

    [ExcludeFromSerialization]
    public ConditionTreeItem? Parent { get; set; }

    [ExcludeFromSerialization]
    [ExcludeFromValidation]
    public bool IsValid { get; private set; } = true;

    public ObservableCollection<ConditionTreeItem> Items { get; private set; }


    public event EventHandler<EventArgs>? Updated;

    private void OnConditionItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var listSender = sender as IList;
        if (listSender is null)
        {
            return;
        }

        if (e.OldItems is not null)
        {
            foreach (var item in e.OldItems)
            {
                var conditionTreeItem = (ConditionTreeItem)item;

                if (ReferenceEquals(conditionTreeItem, this))
                {
                    conditionTreeItem.Parent = null;
                }

                conditionTreeItem.Updated -= OnConditionUpdated;
            }
        }

        var newCollection = (e.Action == NotifyCollectionChangedAction.Reset) ? listSender : e.NewItems;
        if (newCollection is not null)
        {
            foreach (var item in newCollection)
            {
                var conditionTreeItem = (ConditionTreeItem)item;

                conditionTreeItem.Parent = this;
                conditionTreeItem.Updated += OnConditionUpdated;
            }
        }
    }

    protected override void OnDeserialized()
    {
        base.OnDeserialized();

        SubscribeToEvents();

        foreach (var item in Items)
        {
            item.Parent = this;
        }
    }

    protected override void OnValidated(IValidationContext validationContext)
    {
        base.OnValidated(validationContext);

        IsValid = !validationContext.HasErrors;
    }

    private void OnItemsChanged()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        var items = Items;
        if (items is not null)
        {
            items.CollectionChanged += OnConditionItemsCollectionChanged;

            foreach (var item in items)
            {
                item.Updated += OnConditionUpdated;
            }
        }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        RaiseUpdated();
    }

    protected void RaiseUpdated()
    {
        Updated?.Invoke(this, EventArgs.Empty);
    }

    private void OnConditionUpdated(object? sender, EventArgs e)
    {
        RaiseUpdated();
    }

    public abstract bool CalculateResult(object entity);

    protected bool Equals(ConditionTreeItem other)
    {
        return Items.Equals(other.Items);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((ConditionTreeItem)obj);
    }

    public override int GetHashCode()
    {
        return Items.GetHashCode();
    }
}
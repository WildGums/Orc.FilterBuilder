namespace Orc.FilterBuilder.Automation
{
    using System;

    public class FilterSchemeBuilder
    {
        public static FilterSchemeBuilder Start<T>()
        {
            var filterBuilder = StartGroup<T>();
            filterBuilder.ClearItems();
            return filterBuilder;
        }

        public static FilterSchemeBuilder StartGroup<T>(ConditionGroupType groupType = ConditionGroupType.And) 
            => new(typeof(T), groupType);

        private readonly FilterScheme _filterScheme;

        private ConditionGroup? _currentConditionGroup;

        private FilterSchemeBuilder(Type type, ConditionGroupType groupType)
        {
            _filterScheme = new FilterScheme(type);
            _currentConditionGroup = _filterScheme.Root as ConditionGroup;
            if (_currentConditionGroup is not null)
            {
                _currentConditionGroup.Type = groupType;
            }
        }

        public FilterSchemeBuilder Title(string title)
        {
            _filterScheme.Title = title;

            return this;
        }

        public FilterSchemeBuilder And() => Group(ConditionGroupType.And);
        public FilterSchemeBuilder Or() => Group(ConditionGroupType.Or);

        public FilterSchemeBuilder Group(ConditionGroupType type)
        {
            var newGroup = new ConditionGroup { Type = type };

            AddCondition(newGroup);

            _currentConditionGroup = newGroup;


            return this;
        }

        public FilterSchemeBuilder FinishConditionGroup()
        {
            _currentConditionGroup = _currentConditionGroup?.Parent as ConditionGroup;

            return this;
        }

        public FilterSchemeBuilder Property(string name, Condition condition, string? value = default)
        {
            var type = _filterScheme.TargetType;
            var property = type.GetProperty(name);
            if (property is null)
            {
                throw new InvalidOperationException($"Cannot find property '{type.Name}.{name}'");
            }

            var propertyExpression = new PropertyExpression
            {
                Property = new PropertyMetadata(type, property)
            };

            var dataTypeExpression = propertyExpression.DataTypeExpression;
            if (dataTypeExpression is StringExpression expression)
            {
                expression.Value = value ?? string.Empty;
                expression.SelectedCondition = condition;
            }

            AddCondition(propertyExpression);

            return this;
        }

        public FilterSchemeBuilder Property<TValue>(string name, Condition condition, TValue value = default)
            where TValue : struct, IComparable, IFormattable, IComparable<TValue>, IEquatable<TValue>
        {
            var type = _filterScheme.TargetType;
            var property = type.GetProperty(name);
            if (property is null)
            {
                throw new InvalidOperationException($"Cannot find property '{type.Name}.{name}'");
            }

            var propertyExpression = new PropertyExpression
            {
                Property = new PropertyMetadata(type, property)
            };

            var dataTypeExpression = propertyExpression.DataTypeExpression;
            if (dataTypeExpression is ValueDataTypeExpression<TValue> valueExpression)
            {
                valueExpression.Value = value;
                valueExpression.SelectedCondition = condition;
            }

            AddCondition(propertyExpression);

            return this;
        }

        public FilterSchemeBuilder Property(string name, DataTypeExpression expression)
        {
            var type = _filterScheme.TargetType;
            var property = type.GetProperty(name);
            if (property is null)
            {
                throw new InvalidOperationException($"Cannot find property '{type.Name}.{name}'");
            }

            var propertyExpression = new PropertyExpression
            {
                Property = new PropertyMetadata(type, property),
                DataTypeExpression = expression
            };

            AddCondition(propertyExpression);

            return this;
        }

        public FilterScheme ToFilterScheme() => _filterScheme;

        private void AddCondition(ConditionTreeItem condition)
        {
            if (_currentConditionGroup is null)
            {
                _filterScheme.ConditionItems.Add(condition);
            }
            else
            {
                _currentConditionGroup.Items.Add(condition);
            }
        }

        private void ClearItems()
        {
            _filterScheme.ConditionItems.Clear();
            _currentConditionGroup = null;
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// This file contains a lot of copy-paste from https://github.com/Catel/Catel/blob/develop/src/Catel.MVVM/Catel.MVVM.Shared/Windows/Interactivity/Behaviors/AutoCompletion.cs
// Discussion points
// 1. We can not use AutoCompletion from Catel because of its assumption that IEnumerable and property name is enough for getting values;
// 2. Changing Catel.AutoComplete may become too big breaking change.
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using Catel.IoC;
    using Catel.Windows.Input;
    using Catel.Windows.Interactivity;
    using Models;
    using Services;
    using PropertyMetadata = System.Windows.PropertyMetadata;

    public class AutoCompletion : BehaviorBase<TextBox>
    {
        #region Fields
        private readonly IAutoCompletionService _autoCompletionService;
        private readonly ListBox _suggestionListBox;
        private readonly Popup _popup;

        private bool _isUpdatingAssociatedObject;

        private bool _subscribed;
        private string _valueAtSuggestionBoxOpen;
        private string[] _availableSuggestions;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCompletion"/> class.
        /// </summary>
        public AutoCompletion()
        {
            var dependencyResolver = this.GetDependencyResolver();
            _autoCompletionService = dependencyResolver.Resolve<IAutoCompletionService>();

            _suggestionListBox = new ListBox();
            _suggestionListBox.Margin = new Thickness(0d);

            _popup = new Popup();
            _popup.Child = _suggestionListBox;
            _popup.StaysOpen = false;

        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether the auto completion functionality is enabled.
        /// </summary>
        /// <value>The is enabled.</value>
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        /// <summary>
        /// The is enabled property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool),
            typeof(AutoCompletion), new PropertyMetadata(true, (sender, e) => ((AutoCompletion)sender).OnIsEnabledChanged()));

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        /// <summary>
        /// The property name property.
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string),
            typeof(AutoCompletion), new PropertyMetadata(string.Empty, (sender, e) => ((AutoCompletion)sender).OnPropertyNameChanged()));

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// The items source property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable),
            typeof(AutoCompletion), new PropertyMetadata(null, (sender, e) => ((AutoCompletion)sender).OnItemsSourceChanged()));

        public IMetadataProvider MetadataProvider
        {
            get { return (IMetadataProvider)GetValue(MetadataProviderProperty); }
            set { SetValue(MetadataProviderProperty, value); }
        }

        /// <summary>
        /// The items source property.
        /// </summary>
        public static readonly DependencyProperty MetadataProviderProperty = DependencyProperty.Register("MetadataProvider", typeof(IMetadataProvider),
            typeof(AutoCompletion), new PropertyMetadata(null, (sender, e) => ((AutoCompletion)sender).OnMetadataProviderChanged()));



        /// <summary>
        /// Gets or sets a value indicating whether this behavior should use the auto completion service to filter the items source.
        /// <para />
        /// If this value is set to <c>false</c>, it will show the <see cref="ItemsSource"/> as auto completion source without filtering.
        /// <para />
        /// The default value is <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if this behavior should use the auto completion service; otherwise, <c>false</c>.</value>
        public bool UseAutoCompletionService
        {
            get { return (bool)GetValue(UseAutoCompletionServiceProperty); }
            set { SetValue(UseAutoCompletionServiceProperty, value); }
        }

        /// <summary>
        /// The use automatic completion service property.
        /// </summary>
        public static readonly DependencyProperty UseAutoCompletionServiceProperty =
            DependencyProperty.Register("UseAutoCompletionService", typeof(bool), typeof(AutoCompletion), new PropertyMetadata(true));
        #endregion

        #region Methods
        /// <summary>
        /// Called when the associated object is loaded.
        /// </summary>
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            SubscribeEvents();
        }

        /// <summary>
        /// Called when the associated object is unloaded.
        /// </summary>
        protected override void OnAssociatedObjectUnloaded()
        {
            UnsubscribeEvents();

            base.OnAssociatedObjectUnloaded();
        }

        private void SubscribeEvents()
        {
            if (!IsEnabled || _subscribed)
            {
                return;
            }

            var associatedObject = AssociatedObject;
            if (associatedObject != null)
            {
                _subscribed = true;

                associatedObject.TextChanged += OnTextChanged;
                associatedObject.PreviewKeyDown += OnPreviewKeyDown;

                _suggestionListBox.SelectionChanged += OnSuggestionListBoxSelectionChanged;
                _suggestionListBox.MouseLeftButtonUp += OnSuggestionListBoxMouseLeftButtonUp;
                UpdateSuggestionBox(false);
            }
        }

        private void UnsubscribeEvents()
        {
            if (!_subscribed)
            {
                return;
            }

            var associatedObject = AssociatedObject;
            if (associatedObject != null)
            {
                associatedObject.TextChanged -= OnTextChanged;

                associatedObject.PreviewKeyDown -= OnPreviewKeyDown;
                _suggestionListBox.SelectionChanged -= OnSuggestionListBoxSelectionChanged;
                _suggestionListBox.MouseLeftButtonUp -= OnSuggestionListBoxMouseLeftButtonUp;
                _subscribed = false;
            }
        }

        private void UpdateSuggestionBox(bool isVisible)
        {
            var textBox = AssociatedObject;

            if (isVisible && !_popup.IsOpen)
            {
                _valueAtSuggestionBoxOpen = textBox.Text;
            }

            _popup.Width = textBox.ActualWidth;

            _popup.PlacementTarget = textBox;
            _popup.Placement = PlacementMode.Bottom;
            _popup.IsOpen = isVisible;
        }



        private void UpdateSuggestions()
        {
            if (!IsEnabled)
            {
                return;
            }

            var associatedObject = AssociatedObject;
            if (associatedObject == null)
            {
                return;
            }

            var text = AssociatedObject.Text;
            string[] availableSuggestions = null;

            if (ItemsSource != null && MetadataProvider != null)
            {
                if (UseAutoCompletionService)
                {
                    availableSuggestions = _autoCompletionService.GetAutoCompleteValues(PropertyName, text, ItemsSource, MetadataProvider);
                }
                else
                {
                    var items = new List<string>();
                    foreach (var item in ItemsSource)
                    {
                        var itemAsString = item as string;
                        if (!string.IsNullOrWhiteSpace(itemAsString))
                        {
                            items.Add(itemAsString);
                        }
                    }

                    availableSuggestions = items.ToArray();
                }
            }

            _availableSuggestions = availableSuggestions;
            _suggestionListBox.ItemsSource = _availableSuggestions;
        }

        private void OnPropertyNameChanged()
        {
            UpdateSuggestions();
        }

        private void OnItemsSourceChanged()
        {
            UpdateSuggestions();
        }

        private void OnMetadataProviderChanged()
        {
            UpdateSuggestions();
        }

        private void OnIsEnabledChanged()
        {
            if (IsEnabled)
            {
                SubscribeEvents();
            }
            else
            {
                UnsubscribeEvents();
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingAssociatedObject)
            {
                _isUpdatingAssociatedObject = false;
                return;
            }

            UpdateSuggestions();

            if (_availableSuggestions == null || _availableSuggestions.Length == 0)
            {
                UpdateSuggestionBox(false);
            }
            else
            {
                UpdateSuggestionBox(true);
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (KeyboardHelper.AreKeyboardModifiersPressed(ModifierKeys.Control))
                {
                    UpdateSuggestionBox(true);
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Down)
            {
                if (_suggestionListBox.SelectedIndex < _suggestionListBox.Items.Count)
                {
                    _suggestionListBox.SelectedIndex = _suggestionListBox.SelectedIndex + 1;
                }
            }

            if (e.Key == Key.Up)
            {
                if (_suggestionListBox.SelectedIndex > -1)
                {
                    _suggestionListBox.SelectedIndex = _suggestionListBox.SelectedIndex - 1;
                }
            }

            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                // Commit the selection
                UpdateSuggestionBox(false);
                e.Handled = (e.Key == Key.Enter);

                var binding = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
                if (binding != null)
                {
                    binding.UpdateSource();
                }
            }

            if (e.Key == Key.Escape)
            {
                if (_popup.IsOpen)
                {
                    // Cancel the selection
                    UpdateSuggestionBox(false);

                    _isUpdatingAssociatedObject = true;

                    AssociatedObject.Text = _valueAtSuggestionBoxOpen;
                    e.Handled = true;
                }
            }
        }

        private void OnSuggestionListBoxMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateSuggestionBox(false);
        }

        private void OnSuggestionListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var textBox = AssociatedObject;

            if (_suggestionListBox.ItemsSource != null)
            {
                textBox.TextChanged -= OnTextChanged;

                if (_suggestionListBox.SelectedIndex != -1)
                {
                    _isUpdatingAssociatedObject = true;

                    textBox.Text = _suggestionListBox.SelectedItem.ToString();
                }

                textBox.TextChanged += OnTextChanged;
            }
        }
        #endregion
    }
}

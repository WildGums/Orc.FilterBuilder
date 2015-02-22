// Control to present collection of JsonObject. A lot of code is copy pasted from http://blogs.msdn.com/b/carlosfigueira/archive/2010/12/31/jsonvalue-viewer.aspx
// A lot of code is changed/added as well.

namespace Orc.FilterBuilder.AlternativeExample.Controls
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Newtonsoft.Json.Linq;

    public class JsonCollectionViewer: TreeView
    {
        private IEnumerable<object> _source;

        /// <summary>
        /// Defines a property for binding a <see cref="JsonValue"/> value to this control.
        /// </summary>
        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", 
            typeof(IEnumerable<object>), 
            typeof(JsonCollectionViewer),
            new PropertyMetadata(SourceChanged));

        public static DependencyProperty TopItemsProperty = DependencyProperty.Register("TopItems",
            typeof(int),
            typeof(JsonCollectionViewer),
            new UIPropertyMetadata(500));

        /// <summary>
        /// Gets or sets the source for this viewer.
        /// </summary>
        public IEnumerable<object> Source
        {
            get
            {
                return (IEnumerable<object>)this.GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        public int TopItems
        {
            get
            {
                return (int)this.GetValue(TopItemsProperty);
            }
            set
            {
                SetValue(TopItemsProperty, value);
            }
        }

        private static void SourceChanged(
            DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var viewer = sender as JsonCollectionViewer;
            if (viewer == null || e.NewValue == null)
            {
                return;
            }

            if (e.OldValue is ObservableCollection<object>)
            {
                var oldCollection = e.OldValue as ObservableCollection<object>;
                oldCollection.CollectionChanged -= viewer.OnSourceCollectionChanged;
            }

            if (e.NewValue is ObservableCollection<object>)
            {
                var newCollection = e.NewValue as ObservableCollection<object>;
                newCollection.CollectionChanged += viewer.OnSourceCollectionChanged;
            }

            viewer.UpdateTree();
        }

        private void OnSourceCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.UpdateTree();
        }

        private void UpdateTree()
        {
            this.Items.Clear();

            TreeViewItem root = CreateNode(null, this.Source, true);
            this.Items.Add(root);
            AddChildrenNodes(root, this.Source);
        }
        
        private static bool IsComplex(object jsonValue)
        {
            //return (jsonValue != null && jsonValue.JsonType != JsonType.Boolean && jsonValue.JsonType != JsonType.Number && jsonValue.JsonType != JsonType.String);
            return (jsonValue is IEnumerable<object> || jsonValue is Dictionary<string, object>) && !(jsonValue is JValue);
        }

        private TreeViewItem CreateNode(string key, object value, bool isExpanded)
        {
            TreeViewItem result = new TreeViewItem();
            result.IsExpanded = isExpanded;
            StackPanel panel = new StackPanel { Orientation = Orientation.Horizontal };
            string name = null;
            bool addValue = value == null || !IsComplex(value);
            string comment = null;
            bool addComment = false;
            string type = null;
            if (value == null)
            {
                type = "(P)";
                name = " <<null>>";
            }
            else
            {
                if ((value is IEnumerable<object>) && !(value is JValue))
                {
                    var c = value as IEnumerable<object>;
                    var cnt = c.Count();
                    type = value is JArray ? "(A)":"(C)";
                    addComment = (cnt > this.TopItems || type == "(C)");
                    comment = cnt > this.TopItems ? string.Format(" {0} items. Only top {1} is shown.", cnt, this.TopItems) : string.Format(" {0} item(s)", cnt);
                }
                else
                {
                    name = " " + value.ToString();
                }
            }

            panel.Children.Add(new TextBlock { Text = type, Foreground = Brushes.Chocolate });
            if (key != null)
            {
                panel.Children.Add(new TextBlock { Text = " " + key });
            }

            if (addValue)
            {
                panel.Children.Add(new TextBlock { Text = " :" });
                if (name != null)
                {
                    panel.Children.Add(new TextBlock { Text = name });
                }
            }
            else if (addComment)
            {
                panel.Children.Add(new TextBlock { Text = comment, Foreground = Brushes.Gainsboro });
            }

            result.Header = panel;
            return result;


        }

        private void AddChildrenNodes(TreeViewItem root, object value)
        {
            if (value != null && IsComplex(value))
            {
                var enumerable = value as IEnumerable<object>;
                var dictionary = value as Dictionary<string, object>;
                if (enumerable != null)
                {
                    var i = 0;
                    foreach (var item in enumerable.Take(this.TopItems))
                    {
                        TreeViewItem node = CreateNode(string.Format("[{0}]", i++), item, false);
                        root.Items.Add(node);
                        AddChildrenNodes(node, item);
                    }
                }
                else if (dictionary != null)
                {
                    var i = 0;
                    foreach (var kvp in dictionary)
                    {
                        TreeViewItem node = CreateNode(kvp.Key, kvp.Value, false);
                        root.Items.Add(node);
                        AddChildrenNodes(node, kvp.Value);
                    }
                }
            }
        }
    }
}

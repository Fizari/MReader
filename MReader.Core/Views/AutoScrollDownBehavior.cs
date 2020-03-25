using MReader.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace MReader.Core.Views
{
    //***
    // Modified from https://stackoverflow.com/questions/2006729/how-can-i-have-a-listbox-auto-scroll-when-a-new-item-is-added/2007072
    //***
    public class AutoScrollDownBehavior : Behavior<ItemsControl>
    {
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            "Mode", typeof(AutoScrollMode), typeof(AutoScrollDownBehavior), new PropertyMetadata(AutoScrollMode.Enabled));
        public AutoScrollMode Mode
        {
            get => (AutoScrollMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            Clear();
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnloaded;
            base.OnDetaching();
        }

        private static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register(
            "ItemsCount", typeof(int), typeof(AutoScrollDownBehavior), new PropertyMetadata(0, (s, e) => ((AutoScrollDownBehavior)s).OnCountChanged()));
        private ScrollViewer _scroll;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var binding = new Binding("ItemsSource.Count")
            {
                Source = AssociatedObject,
                Mode = BindingMode.OneWay
            };
            _scroll = AssociatedObject.GetDescendantByType(typeof(ScrollViewer)) as ScrollViewer;
            BindingOperations.SetBinding(this, ItemsCountProperty, binding);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            BindingOperations.ClearBinding(this, ItemsCountProperty);
        }

        private void OnCountChanged()
        {
            var mode = Mode;
            if (mode == AutoScrollMode.Enabled)
            {
                _scroll.ScrollToBottom();
            }
        }
    }

    public enum AutoScrollMode
    {
        /// <summary>
        /// No auto scroll
        /// </summary>
        Disabled,
        /// <summary>
        /// Automatically scrolls vertically regardless of where the focus is
        /// </summary>
        Enabled
    }
}

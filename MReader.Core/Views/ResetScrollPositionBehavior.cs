using System;
using System.Collections.Generic;
using System.Text;
using MReader.Core.Extensions;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Data;

namespace MReader.Core.Views
{
    public class ResetScrollPositionBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached(
            "Enabled",
            typeof(bool),
            typeof(ResetScrollPositionBehavior),
            new PropertyMetadata(false,
            (d, e) => ((ResetScrollPositionBehavior)d).OnEnabledChanged(d,e)));

        private bool _isEnabled;

        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        private void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool? isEnabled = e.NewValue as bool?;
            _isEnabled = isEnabled ?? false;
        }
        
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Image image = AssociatedObject.FindChild<Image>("mainImageControl");
            image.TargetUpdated += OnTargetUpdated;
        }

        private void OnTargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (_isEnabled)
                AssociatedObject.ScrollToTop();
        }
    }
}

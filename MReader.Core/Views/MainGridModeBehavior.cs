using MReader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace MReader.Core.Views
{
    //This class is hard coded to fit a grid with 5 columns containing 2 splitters arranged in a certain way. 
    //It is in no case a generic behavior that can be applied to any grid.
    //In the future maybe create custom grid <MainGrid> to limit scope
    class MainGridModeBehavior : Behavior<Grid>
    {
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            "Mode", typeof(ReaderMode), typeof(MainGridModeBehavior), new PropertyMetadata(ReaderMode.Splitters, (d, e) => ((MainGridModeBehavior)d).OnModeChanged(d, e)));

        private Binding[] _savedBindings = new Binding[5];
        private MultiBinding _savedMaxWidth;
        private double _scrollViewerWidth;

        public ReaderMode Mode
        {
            get => (ReaderMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        private void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (AssociatedObject == null)
                return;

            ReaderMode mode = (e.NewValue as ReaderMode?) ?? ReaderMode.Splitters;

            var columnDefs = AssociatedObject.ColumnDefinitions;
            if (columnDefs.Count != 5)
                return;

            var sidePanel1 = columnDefs[0];
            var splitter1 = columnDefs[1];
            var scrollViewer = columnDefs[2];
            var splitter2 = columnDefs[3];
            var sidePanel2 = columnDefs[4];

            if (mode == ReaderMode.SinglePanel)
            {
                //save bindings
                for (int i = 0; i < columnDefs.Count; i++)
                {
                    _savedBindings[i] = BindingOperations.GetBinding(columnDefs[i], ColumnDefinition.WidthProperty);
                }
                _savedMaxWidth = BindingOperations.GetMultiBinding(columnDefs[2], ColumnDefinition.MaxWidthProperty);

                _scrollViewerWidth = scrollViewer.ActualWidth;
                sidePanel1.Width = new GridLength(0);
                splitter1.Width = new GridLength(0);
                scrollViewer.Width = new GridLength(1, GridUnitType.Star);
                scrollViewer.MaxWidth = double.MaxValue; 
                splitter2.Width = new GridLength(0);
                sidePanel2.Width = new GridLength(0);

            }
            if (mode == ReaderMode.Splitters)
            {
                //Retrieve bindings
                for (int i = 0; i < columnDefs.Count; i++)
                {
                    if (_savedBindings[i] != null)
                    {
                        columnDefs[i].SetBinding(ColumnDefinition.WidthProperty, _savedBindings[i]);
                    }
                }
                columnDefs[2].SetBinding(ColumnDefinition.MaxWidthProperty, _savedMaxWidth);

                sidePanel1.Width = new GridLength(1, GridUnitType.Star);
                scrollViewer.Width = new GridLength(_scrollViewerWidth);//take that out and the layout breaks. No idea why yet...
                sidePanel2.Width = new GridLength(1, GridUnitType.Star);
            }
        }
    }
}

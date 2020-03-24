using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace MReader.Core.Converters
{
    public class ImagePanelWidthConverter : IMultiValueConverter
    {
        //value[0] should be the grid width in DOUBLE
        //value[1] should be splitter width in DOUBLE
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                return 100000;
            }
            double res_width = System.Convert.ToDouble(values[0]) - (System.Convert.ToDouble(values[1]) * 2);
            return res_width;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}

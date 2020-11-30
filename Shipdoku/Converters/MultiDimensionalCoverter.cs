using Shipdoku.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Shipdoku.Converters
{
    class MultiDimensionalCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var path = new ShipdokuFieldToImageConverter().Convert((values[0] as EShipdokuField[,])[(int)values[1], (int)values[2]], null, null, null);
            return new BitmapImage(new Uri(path.ToString(), UriKind.Relative));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

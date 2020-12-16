using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Shipdoku.Enums;

namespace Shipdoku.Converters
{
    class MultiDimensionalCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var path = new ShipdokuFieldToImageConverter().Convert(((EShipdokuField[,]) values[0])[(int)values[1], (int)values[2]], null, null, null);
            return new BitmapImage(new Uri("/" + path, UriKind.Relative));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

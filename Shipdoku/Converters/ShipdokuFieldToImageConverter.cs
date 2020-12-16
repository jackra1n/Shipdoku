using Shipdoku.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Shipdoku.Converters
{
    public class ShipdokuFieldToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EShipdokuField shipdokuField = (EShipdokuField) value;
            string imagesDirectory = "Images";
            switch (shipdokuField)
            {
                case EShipdokuField.Water:
                    return imagesDirectory + "/water.png";
                case EShipdokuField.ShipMiddle:
                    return imagesDirectory + "/shipMiddle.png";
                case EShipdokuField.ShipDown:
                    return imagesDirectory + "/shipDown.png";
                case EShipdokuField.ShipLeft:
                    return imagesDirectory + "/shipLeft.png";
                case EShipdokuField.ShipUp:
                    return imagesDirectory + "/shipUp.png";
                case EShipdokuField.ShipRight:
                    return imagesDirectory + "/shipRight.png";
                case EShipdokuField.ShipSingle:
                    return imagesDirectory + "/shipSingle.png";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

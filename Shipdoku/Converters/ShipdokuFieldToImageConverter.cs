using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Shipdoku.Enums;

namespace Shipdoku.Converters
{
    public class ShipdokuFieldToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EShipdokuField shipdokuField = (EShipdokuField) value;
            switch (shipdokuField)
            {
                case EShipdokuField.Water:
                    return "/Images/water.png";
                case EShipdokuField.ShipMiddle:
                    return "/Images/shipMiddle.png";
                case EShipdokuField.ShipDown:
                    return "/Images/shipDown.png";
                case EShipdokuField.ShipLeft:
                    return "/Images/shipLeft.png";
                case EShipdokuField.ShipUp:
                    return "/Images/shipUp.png";
                case EShipdokuField.ShipRight:
                    return "/Images/shipRight.png";
                case EShipdokuField.ShipSingle:
                    return "/Images/shipSingle.png";
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

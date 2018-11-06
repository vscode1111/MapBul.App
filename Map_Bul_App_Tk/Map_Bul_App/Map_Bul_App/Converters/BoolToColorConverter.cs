using System;
using System.Globalization;
using Map_Bul_App.Design;
using Xamarin.Forms;

namespace Map_Bul_App.Converters
{
    class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return Color.White;
            var bValue = (bool)value;
            return bValue ? CustomColors.Orange : Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Color)) return false;
            var colorValue = (Color)value;
            return colorValue != Color.White;
        }
    }
}

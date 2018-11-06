using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Map_Bul_App.Converters
{
    class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is List<string>)) return string.Empty;
            var listStringValue = (List<string>)value;
            var result = listStringValue.Aggregate("", (current, item) => current + ("#" + item+" "));
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) return new List<string>();
            var stringValue = (string) value;
            var result = stringValue.Replace("#","").Split(' ');
            return result;
        }
    }
}

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Map_Bul_App.Converters
{
    class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int)) return 0;
            var intValue = (int)value;
            var resultString = ResX.TextResource.FilterFooterShow + " (" + intValue + ")";
            return resultString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

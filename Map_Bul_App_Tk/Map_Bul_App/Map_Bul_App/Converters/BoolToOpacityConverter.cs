using System;
using System.Globalization;
using Xamarin.Forms;

namespace Map_Bul_App.Converters
{
    class BoolToOpacityConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return 1;
            var bValue = (bool)value;
            return bValue ? 1 : 0.5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    class BoolToSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return "acc_open.png";
            var bValue = (bool)value;
            return bValue ? "acc_close.png" : "acc_open.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

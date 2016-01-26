using System;
using Windows.UI.Xaml.Data;

namespace PgenUWP.Converters
{
    public sealed class IntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (double)(int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int)(double)value;
        }
    }
}

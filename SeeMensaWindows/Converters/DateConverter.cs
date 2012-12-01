using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace SeeMensaWindows.Converters
{
    /// <summary>
    /// Converter class for a date.
    /// </summary>
    public class DateConverter : IValueConverter
    {
        /// <summary>
        /// German format provider.
        /// </summary>
        private static IFormatProvider germanFormatProvider = new CultureInfo("de");

        /// <summary>
        /// Converts a date to the correct localized format.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Format(germanFormatProvider, "{0:dd}. {0:MMMM}", value, value);
        }

        /// <summary>
        /// ConvertBack not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

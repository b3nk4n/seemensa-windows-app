using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace SeeMensaWindows.Converters
{
    /// <summary>
    /// Converter class for a day.
    /// </summary>
    public class DayConverter : IValueConverter
    {
        /// <summary>
        /// German format provider.
        /// </summary>
        private static IFormatProvider germanFormatProvider = new CultureInfo("de");

        /// <summary>
        /// Converts a DateTime into the correct localalized format for a day of week.
        /// The day now will be shown as "today" or "heute".
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime day = (DateTime)value;

            CultureInfo ci = CultureInfo.CurrentCulture;

            if (DateTime.Now.Date == day.Date)
            {
                return "Heute";
            }

            return string.Format(germanFormatProvider, "{0:dddd}", value);
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

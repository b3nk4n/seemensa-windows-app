using System;

namespace SeeMensaWindows.Helpers
{
    public static class Converter
    {
        /// <summary>
        /// Converts a UNIX timestamp in a DateTime object.
        /// </summary>
        /// <param name="timestamp">The timepsamp as a string.</param>
        /// <returns>The converted DateTime object.</returns>
        public static DateTime TimestampToDate(string timestamp)
        {
            //  gerechnet wird ab der UNIX Epoche (+12h and +2h for GMT+2)
            DateTime dateTime = new DateTime(1970, 1, 1, 14, 0, 0, 0);
            // den Timestamp addieren           
            dateTime = dateTime.AddSeconds(Int32.Parse(timestamp));

            return dateTime;
        }
    }
}

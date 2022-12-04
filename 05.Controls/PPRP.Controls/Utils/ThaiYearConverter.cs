#region Using

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace PPRP.Controls.Utils
{
    /// <summary>
    /// The Thai Year Converter class.
    /// </summary>
    public class ThaiYearConverter : IValueConverter
    {
        //private static ThaiBuddhistCalendar ThaiCalendar = new ThaiBuddhistCalendar();
        public static CultureInfo ThaiCultureInfo = new CultureInfo("th-TH");

        public static class Helpers
        {
            public static int GetMonthIndex(string month)
            {
                return Array.FindIndex(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames,
                                 t => t.Equals(month, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        /// <summary>
        /// Convert.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (null != culture) culture.DateTimeFormat.Calendar = thaicalendar;
            string[] values = (null != value) ? value.ToString().Split(' ') : null;
            string ret = value.ToString();
            if (null != values && values.Length >= 2)
            {
                int mn = Helpers.GetMonthIndex(values[0]);
                string thMonth = ThaiCultureInfo.DateTimeFormat.GetMonthName(mn);
                int yr = int.Parse(values[1]);
                if (yr < 2500) yr += 543;
                ret = thMonth + " " + yr.ToString();
            }
            return ret;
        }
        /// <summary>
        /// ConvertBack.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (null != culture) culture.DateTimeFormat.Calendar = thaicalendar;
            throw new NotImplementedException();
        }
    }
}


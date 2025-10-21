using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DemoExam
{
    public class DateToDatePickerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DateTime.UtcNow;

            if (value is DateTime date)
            {
                if (date.Kind != DateTimeKind.Utc)
                    date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                return date.ToUniversalTime();
            }
            if (value is string str && DateTime.TryParse(str, out DateTime result)) return DateTime.SpecifyKind(result, DateTimeKind.Utc);

            return DateTime.UtcNow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DateTime.UtcNow;

            if (value is DateTime date)
            {
                if (date.Kind != DateTimeKind.Utc)
                    date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                return date.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture);
            }

            return DateTime.UtcNow;
        }
    }
}

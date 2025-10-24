using DemoExam.Models;
using Microsoft.Extensions.Options;
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
    class FiltersVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Application.Current.Properties["CurrentUser"] == null)
            {
                return new GridLength(0);
            }
            var user = (User)Application.Current.Properties["CurrentUser"];
            if (user.role != "Авторизированный клиент")
            {
                return new GridLength(1, GridUnitType.Star);
            }
            return new GridLength(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

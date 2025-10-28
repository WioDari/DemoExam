using DemoExam.Models;
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
    class TovarDeleteVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Tovar tovar)
            {
                if (tovar.id != 0) 
                {
                    var context = new AppDbContext();
                    var orders = context.Order.Where(o => o.art.Contains(tovar.art)).ToList();
                    if (orders.Count > 0) return Visibility.Collapsed;
                    return Visibility.Visible;
                }
                else return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

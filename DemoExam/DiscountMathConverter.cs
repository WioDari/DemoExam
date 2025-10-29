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
    class DiscountMathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id)
            {
                var context = new AppDbContext();
                Tovar tovar = context.Tovar.FirstOrDefault(t => t.id == id);
                double discounted = tovar.price * (1 - (double)tovar.discount / 100);
                return discounted.ToString("0.00");
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

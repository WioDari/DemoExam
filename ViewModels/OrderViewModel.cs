using DemoExam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExam.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }

        public class OrderView
        {
            public string id { get; set; }
            public string address { get; set; }
            public string art { get; set; }
            public string order_date { get; set; }
            public string ship_date { get; set; }
            public string status { get; set; }
        }

        public ObservableCollection<OrderView> data { get; set; }


        public OrderViewModel()
        {
            LoadOrders();
        }

        public void LoadOrders()
        {
            var context = new AppDbContext();
            data = new ObservableCollection<OrderView>(context.Order
                .Include(o => o.address)
                .Select(o => new OrderView
                {
                    id = o.id.ToString(),
                    address = o.address.address,
                    art = o.art,
                    order_date = o.order_date,
                    ship_date = o.ship_date,
                    status = o.status
                })
                .ToList());
        }
    }
}

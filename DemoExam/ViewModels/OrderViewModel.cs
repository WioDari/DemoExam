using DemoExam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        private OrderView _selectedOrder;
        public OrderView SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                if (_selectedOrder != null)
                    EditOrder(int.Parse(_selectedOrder.id));
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

        private ObservableCollection<OrderView> _data;
        public ObservableCollection<OrderView> data { 
            get => _data; 
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        public OrderViewModel()
        {
            LoadOrders();
        }

        public void EditOrder(int? order_id = null)
        {
            var w = new Views.EditOrder();
            w.DataContext = new EditOrderViewModel(order_id);
            w.ShowDialog();
            LoadOrders();
        }

        public void LoadOrders()
        {
            var context = new AppDbContext();
            _data = new ObservableCollection<OrderView>(context.Order
                .Include(o => o.address)
                .Select(o => new OrderView
                {
                    id = o.id.ToString(),
                    address = o.address.address,
                    art = o.art,
                    order_date = o.order_date.ToString("dd.MM.yyyy"),
                    ship_date = o.ship_date.ToString("dd.MM.yyyy"),
                    status = o.status
                })
                .OrderBy(o => o.id)
                .ToList());
        }
    }
}

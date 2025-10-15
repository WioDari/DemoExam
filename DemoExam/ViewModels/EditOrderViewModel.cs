using DemoExam.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DemoExam.ViewModels
{
    public class EditOrderViewModel : BaseViewModel
    {
        private string _art;
        public string art
        {
            get => _art;
            set
            {
                _art = value;
                OnPropertyChanged();
            }
        }

        private string _status;
        public string status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private string _address;
        public string address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private DateOnly _order_date;
        public DateOnly order_date
        {
            get => _order_date;
            set
            {
                _order_date = value;
                OnPropertyChanged();
            }
        }

        private DateOnly _ship_date;
        public DateOnly ship_date
        {
            get => _ship_date;
            set
            {
                _ship_date = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public string title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Address> _addressList;
        public ObservableCollection<Address> addressList
        {
            get => _addressList;
            set
            {
                _addressList = value;
                OnPropertyChanged();
            }
        }

        private int _selectedAddressId;
        public int selectedAddressId
        {
            get => _selectedAddressId;
            set
            {
                _selectedAddressId = value;
                OnPropertyChanged();
            }
        }

        private List<string> _statusList = new List<string>(["Новый", "Завершен"]);
        public List<string> statusList
        {
            get => _statusList;
        }

        private string _selectedStatus;
        public string selectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
            }
        }


        public EditOrderViewModel(int? order_id = null)
        {
            var context = new AppDbContext();
            addressList = new ObservableCollection<Address>(context.Address.ToList());
            if (order_id == null)
            {
                title = "Создание заказа";
            }
            else
            {
                title = "Редактирование заказа";
                var order = context.Order.FirstOrDefault(o => o.id == order_id);
                art = order.art;
                selectedStatus = order.status;
                selectedAddressId = order.addressid;
                order_date = DateOnly.ParseExact(order.order_date, "dd.mm.yyyy");
                ship_date = DateOnly.ParseExact(order.ship_date, "dd.mm.yyyy");
            }
        }
    }
}

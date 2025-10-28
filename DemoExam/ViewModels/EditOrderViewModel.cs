using DemoExam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
                _order.art = value;
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
                _order.status = selectedStatus;
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

        private DateTime _order_date = DateTime.UtcNow;
        public DateTime order_date
        {
            get => _order_date;
            set
            {
                _order_date = value;
                _order.order_date = value.ToUniversalTime();
                OnPropertyChanged();
            }
        }

        private DateTime _ship_date = DateTime.UtcNow;
        public DateTime ship_date
        {
            get => _ship_date;
            set
            {
                _ship_date = value;
                _order.ship_date = value.ToUniversalTime();
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
                _order.addressid = _selectedAddressId;
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
                _order.status = _selectedStatus;
                OnPropertyChanged();
            }
        }

        private Order _order;
        public Order orderContainer
        {
            get => _order;
        }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public EditOrderViewModel(int? order_id = null)
        {
            SaveCommand = new RelayCommand(SaveData);
            DeleteCommand = new RelayCommand(DeleteData);

            var context = new AppDbContext();
            addressList = new ObservableCollection<Address>(context.Address.ToList());

            _order = order_id == null ? new Order() : context.Order.FirstOrDefault(o => o.id == order_id);

            if (order_id == null)
            {
                title = "Создание заказа";
                var user = (User)Application.Current.Properties["CurrentUser"];
                _order.client = user.name;
            }
            else
            {
                title = "Редактирование заказа";
                var order = context.Order.FirstOrDefault(o => o.id == order_id);
                art = order.art;
                selectedStatus = order.status;
                selectedAddressId = order.addressid;
                order_date = order.order_date.ToUniversalTime();
                ship_date = order.ship_date.ToUniversalTime();
            }
        }

        public void DeleteData()
        {
            var context = new AppDbContext();
            if (_order.id == null)
            {
                MessageBox.Show("Заказ не выбран!", "Ошибка");
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is Views.EditOrder)
                    {
                        w.Close();
                    }
                }
                return;
            }
            
            var result = MessageBox.Show("Вы уверены, что хотите удалить заказ?","Подтверждение",MessageBoxButton.YesNo,MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                context.Order.Remove(_order);
                context.SaveChanges();
                MessageBox.Show("Заказ успешно удалён!", "Успешно");
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is Views.EditOrder)
                    {
                        w.Close();
                    }
                }
                return;
            }
        }

        public void SaveData()
        {
            MessageBox.Show(order_date.ToString());
            var context = new AppDbContext();
            if (_order.id == 0)
            {
                context.Order.Add(_order);
            }
            else
            {
                context.Order.Update(_order);
            }
            context.SaveChanges();
            MessageBox.Show("Изменения сохранены!", "Успешно");
            
            foreach(Window w in Application.Current.Windows)
            {
                if (w is Views.EditOrder)
                {
                    w.Close();
                }
            }
        }
    }
}

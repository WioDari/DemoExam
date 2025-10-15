using DemoExam.Models;
using DemoExam.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace DemoExam.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private string _name;
        public string name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private Page _currentPage;
        public Page currentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogoutCommand { get; }
        public ICommand TovarNavigate { get; }
        public ICommand OrderNavigate { get; }
        public ICommand EditOrderCommand { get; }

        public MenuViewModel()
        {
            User user = (User)Application.Current.Properties["CurrentUser"];
            name = user.name;
            currentPage = new Views.Tovar();
            LogoutCommand = new RelayCommand(Logout);
            TovarNavigate = new RelayCommand(TovarNav);
            OrderNavigate = new RelayCommand(OrderNav);
            EditOrderCommand = new RelayCommand(EditOrder);
        }

        public void EditOrder()
        {
            EditOrder editOrder = new EditOrder();
            editOrder.DataContext = new EditOrderViewModel();
            editOrder.ShowDialog();
        }

        public void Logout()
        {
            Application.Current.Properties["CurrentUser"] = null;

            Window auth = new Auth();
            auth.Show();
            foreach(Window w in Application.Current.Windows)
            {
                if (w is not Views.Auth)
                {
                    w.Close();
                }
            }
        }

        public void TovarNav()
        {
            currentPage = new Views.Tovar();
        }

        public void OrderNav()
        {
            currentPage = new Views.Order();
        }
    }
}

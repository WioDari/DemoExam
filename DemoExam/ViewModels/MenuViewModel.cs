using DemoExam.Models;
using DemoExam.Properties;
using DemoExam.Timer;
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
        private string _timeRemaining;
        public string TimeRemaining
        {
            get => _timeRemaining;
            set
            {
                _timeRemaining = value;
                OnPropertyChanged();
            }
        }

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

        private string _header = "Список товаров";
        public string header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogoutCommand { get; }
        public ICommand TovarNavigate { get; }
        public ICommand OrderNavigate { get; }
        public ICommand EditOrderCommand { get; }

        public ICommand EditTovarCommand { get; }

        public MenuViewModel()
        {
            App.SessionTimer.Start();

            User user = (User)Application.Current.Properties["CurrentUser"];
            name = user.name;
            currentPage = new Views.Tovar();
            LogoutCommand = new RelayCommand(Logout);
            TovarNavigate = new RelayCommand(TovarNav);
            OrderNavigate = new RelayCommand(OrderNav);
            EditOrderCommand = new RelayCommand(EditOrder);
            EditTovarCommand = new RelayCommand(EditTovar);

            App.SessionTimer.timeRemaining += SessionTimer_timeRemaining;
        }

        private void SessionTimer_timeRemaining(TimeSpan time)
        {
            TimeRemaining = $"({time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2})";
        } 

        public void EditOrder()
        {
            EditOrder editOrder = new EditOrder();
            editOrder.DataContext = new EditOrderViewModel();
            editOrder.ShowDialog();
        }

        public void EditTovar()
        {
            EditTovar editTovar = new EditTovar();
            editTovar.DataContext = new EditTovarViewModel();
            editTovar.ShowDialog();
        }

        public void Logout()
        {
            Application.Current.Properties["CurrentUser"] = null;

            Settings.Default.Reset();
            Settings.Default.Save();

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
            header = "Список товаров";
        }

        public void OrderNav()
        {
            currentPage = new Views.Order();
            header = "Список заказов";
        }
    }
}

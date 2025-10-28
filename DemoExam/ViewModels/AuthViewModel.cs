using DemoExam.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DemoExam.ViewModels
{
    public class AuthViewModel : BaseViewModel
    {
        private string _login = "94d5ous@gmail.com";
        public string login
        {
            get => _login;
            set
            {
                _login = value;
            }
        }

        private string _password = "uzWC67";
        public string password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }
        public ICommand LoginCommand { get; }
        public ICommand OpenGuestCommand { get; }
        public AuthViewModel()
        {
            LoginCommand = new RelayCommand(OnLogin);
            OpenGuestCommand = new RelayCommand(OpenGuest);
        }

        public void OpenGuest()
        {
            Window guest = new Guest();
            guest.Show();
        }

        public void OnLogin()
        {
            var context = new AppDbContext();
            var user = context.User.FirstOrDefault(u => u.login == login && u.password == password);
            if (user == null)
            {
                MessageBox.Show("Логин или пароль введены неправильно!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show($"Добро пожаловать, {user.name}!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.Properties["CurrentUser"] = user;

            Window menu = new Menu();
            menu.Show();
            foreach(Window w in Application.Current.Windows)
            {
                if (w is not Views.Menu)
                {
                    w.Close();
                }
            }
        }
    }
}

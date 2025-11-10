using DemoExam.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DemoExam.Properties;

namespace DemoExam.ViewModels
{
    public class AuthViewModel : BaseViewModel
    {
        private ImageSource _captchaImage;
        public ImageSource captchaImage
        {
            get => _captchaImage;
            set
            {
                _captchaImage = value;
                OnPropertyChanged();
            }
        }

        private string _captchaInput;
        public string captchaInput
        {
            get => _captchaInput;
            set
            {
                _captchaInput = value;
                OnPropertyChanged();
            }
        }

        private string _captchaText;
        public string captchaText
        {
            get => _captchaText;
            set
            {
                _captchaText = value;
                OnPropertyChanged();
            }
        }

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
        public ICommand UpdateCaptchaCommand { get; }

        public AuthViewModel()
        {
            GenerateCaptcha();

            LoginCommand = new RelayCommand(OnLogin);
            OpenGuestCommand = new RelayCommand(OpenGuest);
            UpdateCaptchaCommand = new RelayCommand(GenerateCaptcha);

            if (Settings.Default.userId != 0)
            {
                int userId = Settings.Default.userId;
                var context = new AppDbContext();
                var user = context.User.FirstOrDefault(u => u.id == userId);
                Application.Current.Properties["CurrentUser"] = user;

                new Menu().Show();
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is not Views.Menu)
                    {
                        w.Close();
                    }
                }
            }
        }

        public void OpenGuest()
        {
            Window guest = new Guest();
            guest.Show();
        }

        public void OnLogin()
        {
            if (!string.Equals(_captchaInput, _captchaText))
            {
                MessageBox.Show("Каптча введена неправильно!");
                GenerateCaptcha();
                return;
            }

            var context = new AppDbContext();
            var user = context.User.FirstOrDefault(u => u.login == login && u.password == password);
            if (user == null)
            {
                MessageBox.Show("Логин или пароль введены неправильно!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show($"Добро пожаловать, {user.name}!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.Properties["CurrentUser"] = user;

            Settings.Default.userId = user.id;
            Settings.Default.Save();

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

        public void GenerateCaptcha()
        {
            captchaText = GenerateRandomString(5);
            captchaImage = GenerateCaptchaImage(captchaText);
        }

        public string GenerateRandomString(int len)
        {
            string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghjklmnopqrsftuvwxyz0123456789";
            string result = "";
            for (int i=0;i<len;i++)
            {
                result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            return result;
        }

        public ImageSource GenerateCaptchaImage(string text)
        {
            int width = 200;
            int height = 50;
            var rnd = new Random();
            var image = new DrawingVisual();

            using (var dc = image.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));

                double x = 10;
                foreach(char c in text)
                {
                    var fontSize = rnd.Next(20, 40);
                    var typeface = new Typeface("Times New Roman");
                    var fontColor = Color.FromRgb((byte)rnd.Next(100,255),
                                                  (byte)rnd.Next(100,255),
                                                  (byte)rnd.Next(100,255));
                    var formatted = new FormattedText(c.ToString(),
                                              CultureInfo.InvariantCulture,
                                              FlowDirection.LeftToRight,
                                              typeface,
                                              fontSize, new SolidColorBrush(fontColor));
                    var g = new GeometryDrawing(Brushes.Black, null, formatted.BuildGeometry(new Point(0,0)));
                    var transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new RotateTransform(rnd.NextDouble() * 13 - 9));
                    transformGroup.Children.Add(new SkewTransform(rnd.NextDouble() * 19 - 2, rnd.NextDouble() * 19 - 2));
                    transformGroup.Children.Add(new TranslateTransform(x, (height - fontSize) / 2 + rnd.NextDouble() * 10 - 5));
                    dc.PushTransform(transformGroup);
                    dc.DrawGeometry(Brushes.Black, null, formatted.BuildGeometry(new Point(0,0)));
                    dc.Pop();
                    x += formatted.Width + rnd.Next(2,10);
                }

                for (int i = 0; i < 100; i++)
                {
                    var px = RandomNumberGenerator.GetInt32(width);
                    var py = RandomNumberGenerator.GetInt32(height);
                    dc.DrawEllipse(Brushes.Gray, null, new Point(px, py), 1, 1);
                }
                for (int i = 0; i < RandomNumberGenerator.GetInt32(10); i++)
                {
                    var pen = new Pen(new SolidColorBrush(Color.FromRgb((byte)RandomNumberGenerator.GetInt32(255), 
                                                                        (byte)RandomNumberGenerator.GetInt32(255), 
                                                                        (byte)RandomNumberGenerator.GetInt32(255))), 1);
                    dc.DrawLine(pen, 
                                new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)),
                                new Point(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)));
                }
            }

            var bmp = new RenderTargetBitmap(width, height, 100, 100, PixelFormats.Pbgra32);
            bmp.Render(image);
            return bmp;
        }
    }
}

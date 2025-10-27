using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DemoExam.ViewModels
{
    public class EditTovarViewModel : BaseViewModel
    {
        private string _imagePath { get; set; }
        public string imagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectImageCommand { get; }

        public EditTovarViewModel()
        {
            string defaultImage = Path.Combine("\\Resources\\Images", "picture.png");
            imagePath = defaultImage;
            SelectImageCommand = new RelayCommand(SelectImage);
        }

        private void SelectImage()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Изображения |*.jpg;*.jpeg;*.png; |Все файлы |*.*",
                Title = "Выберите изображение товара"
            };
            if (dlg.ShowDialog() == true)
            {
                string imagesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Images");
                Directory.CreateDirectory(imagesDir);
                string fileName = Path.GetFileName(dlg.FileName);
                string newPath = Path.Combine(imagesDir, fileName);
                File.Copy(dlg.FileName, newPath, overwrite: true);

                imagePath = newPath;
            }
        }
    }
}

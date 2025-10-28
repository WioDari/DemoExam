using DemoExam.Models;
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
        private int _id { get; set; }
        public int id
        {
            get => _id;
            set
            {
                _id = value;
                selectedTovar.id = value;
                OnPropertyChanged();
            }
        }

        private string _art { get; set; }
        public string art
        {
            get => _art;
            set
            {
                _art = value;
                selectedTovar.art = value;
                OnPropertyChanged();
            }
        }

        private string _name { get; set; }
        public string name
        {
            get => _name;
            set
            {
                _name = value;
                selectedTovar.name = value;
                OnPropertyChanged();
            }
        }

        private string _supplier { get; set; }
        public string supplier
        {
            get => _supplier;
            set
            {
                _supplier = value;
                selectedTovar.supplier = value;
                OnPropertyChanged();
            }
        }

        private double _price { get; set; }
        public double price
        {
            get => _price;
            set
            {
                _price = value;
                selectedTovar.price = value;
                OnPropertyChanged();
            }
        }

        private int _quantity { get; set; }
        public int quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                selectedTovar.quantity = value;
                OnPropertyChanged();
            }
        }

        private int _discount { get; set; }
        public int discount
        {
            get => _discount;
            set
            {
                _discount = value;
                selectedTovar.discount = value;
                OnPropertyChanged();
            }
        }
        private string _dim { get; set; }
        public string dim
        {
            get => _dim;
            set
            {
                _dim = value;
                selectedTovar.dim = value;
                OnPropertyChanged();
            }
        }

        private string _description { get; set; }
        public string description
        {
            get => _description;
            set
            {
                _description = value;
                selectedTovar.description = value;
                OnPropertyChanged();
            }
        }

        private List<string> _categoryList = new List<string>(["Мужская обувь", "Женская обувь"]);
        public List<string> categoryList
        {
            get => _categoryList;
        }

        private string _selectedCategory;
        public string selectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                selectedTovar.category = value;
                OnPropertyChanged();
            }
        }

        private string _selectedCreator { get; set; }
        public string selectedCreator
        {
            get => _selectedCreator;
            set
            {
                _selectedCreator = value;
                selectedTovar.creator = value;
                OnPropertyChanged();
            }
        }

        private List<string> _creatorList { get; set; }
        public List<string> creatorList
        {
            get => _creatorList;
            set
            {
                _creatorList = value;
                OnPropertyChanged();
            }
        }

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

        public Tovar selectedTovar { get; set; }

        public ICommand SelectImageCommand { get; }
        public ICommand DeleteTovarCommand { get; }
        public ICommand SaveTovarCommand { get; }

        public EditTovarViewModel(int? tovar_id = null)
        {
            var context = new AppDbContext();
            creatorList = context.Tovar
                .Select(t => t.creator)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            string defaultImage = Path.Combine("\\Resources\\Images", "picture.png");
            imagePath = defaultImage;
            SelectImageCommand = new RelayCommand(SelectImage);

            var tovar = tovar_id == null ? new Tovar() : context.Tovar.FirstOrDefault(t => t.id == tovar_id);
            selectedTovar = tovar;
            if (tovar_id != null)
            {
                id = tovar.id;
                art = tovar.art;
                description = tovar.description;
                price = tovar.price;
                discount = tovar.discount;
                name = tovar.name;
                supplier = tovar.supplier;
                dim = tovar.dim;
                selectedCategory = tovar.category;
                selectedCreator = tovar.creator;
                imagePath = Path.Combine("\\Resources\\Images", tovar.photo);
                quantity = tovar.quantity;
            }

            DeleteTovarCommand = new RelayCommand(DeleteTovar);
            SaveTovarCommand = new RelayCommand(SaveTovar);
        }

        private void DeleteTovar()
        {
            var context = new AppDbContext();
            if (selectedTovar.id != 0)
            {
                var result = MessageBox.Show("?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                if (result == MessageBoxResult.Yes)
                {
                    context.Tovar.Remove(selectedTovar);
                    context.SaveChanges();
                    MessageBox.Show("","");
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w is Views.EditTovar)
                        {
                            w.Close();
                        }
                    }
                    return;
                }
            }
        }

        private void SaveTovar()
        {
            var context = new AppDbContext();
            if (selectedTovar.id != 0)
            {
                context.Tovar.Update(selectedTovar);
                MessageBox.Show("Успешно!", "Изменения сохранены");
            }
            else
            {
                context.Tovar.Add(selectedTovar);
                MessageBox.Show("Успешно!", "Товар добавлен");
            }
            context.SaveChanges();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Views.EditTovar)
                {
                    w.Close();
                }
            }
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
                selectedTovar.photo = imagePath;
            }
        }
    }
}

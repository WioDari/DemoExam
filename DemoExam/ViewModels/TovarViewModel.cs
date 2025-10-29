using DemoExam.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DemoExam.ViewModels
{
    public class TovarViewModel : BaseViewModel
    {
        private string _searchText { get; set; } = string.Empty;
        public string searchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                ApplyFilters();
                OnPropertyChanged();
            }
        }

        private string _selectedSupplier { get; set; } = "Все поставщики";
        public string selectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                ApplyFilters();
                OnPropertyChanged();
            }
        }

        private string _selectedSort { get; set; }
        public string selectedSort
        {
            get => _selectedSort;
            set
            {
                _selectedSort = value;
                ApplyFilters();
                OnPropertyChanged();
            }
        }

        private Tovar _selectedTovar;
        public Tovar selectedTovar
        {
            get => _selectedTovar;
            set
            {
                _selectedTovar = value;
                if (_selectedTovar != null)
                    EditTovar(_selectedTovar.id);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> suppliers { get; set; }

        private ObservableCollection<Tovar> _tovary;
        public ObservableCollection<Tovar> tovary
        {
            get => _tovary;
            set
            {
                _tovary = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Tovar> _allTovary;

        public TovarViewModel()
        {
            LoadTovar();

            var context = new AppDbContext();
            var supplierList = context.Tovar
                .Select(t => t.supplier)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            supplierList.Insert(0, "Все поставщики");
            suppliers = new ObservableCollection<string>(supplierList);
        }

        private void EditTovar(int? tovar_id = null) 
        {
            var user = (User)Application.Current.Properties["CurrentUser"];
            if (user != null && user.role == "Администратор")
            {
                var EditTovar = new Views.EditTovar();
                EditTovar.DataContext = new EditTovarViewModel(tovar_id);
                EditTovar.ShowDialog();
                LoadTovar();
            }
        }

        private void LoadTovar()
        {
            var context = new AppDbContext();
            _allTovary = new ObservableCollection<Tovar>(context.Tovar.ToList());
            _tovary = new ObservableCollection<Tovar>(_allTovary);
            foreach (var t in tovary)
            {
                int rnd = RandomNumberGenerator.GetInt32(100);
                t.name = $"{t.category} | {t.name}";
                t.description = $"Описание товара: {t.description}";
                t.creator = $"Производитель: {t.creator}";
                t.photo = t.photo is null ? $"/Resources/Images/picture.png" : t.photo;
            }
        }

        private void ApplyFilters()
        {
            var query = _allTovary.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string temp = searchText.ToLower();
                query = query.Where(t =>
                t.description.ToLower().Contains(temp) ||
                t.creator.ToLower().Contains(temp) || 
                t.name.ToLower().Contains(temp) ||
                t.category.ToLower().Contains(temp));
            }

            if (selectedSupplier != "Все поставщики")
            {
                query = query.Where(t => t.supplier == selectedSupplier);
            }

            query = selectedSort switch
            {
                "по возрастанию" => query.OrderBy(t => t.quantity),
                "по убыванию" => query.OrderByDescending(t => t.quantity),
                _ => query
            };

            tovary.Clear();
            foreach(var t in query)
            {
                tovary.Add(t);
            }
        }
    }
}

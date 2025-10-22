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
    public class TovarViewModel : BaseViewModel
    {
        private string _searchText { get; set; }
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

        private void LoadTovar()
        {
            var context = new AppDbContext();
            _allTovary = new ObservableCollection<Tovar>(context.Tovar.ToList());
            _tovary = _allTovary;
            foreach (var t in tovary)
            {
                t.name = $"{t.category} | {t.name}";
                t.description = $"Описание товара: {t.description}";
                t.creator = $"Производитель: {t.creator}";
                t.supplier = $"Поставщик: {t.supplier}";
                t.photo = t.photo is null ? $"/Resources/Images/picture.png" : $"/Resources/Images/{t.photo}";
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

            tovary.Clear();
            foreach(var t in query)
            {
                tovary.Add(t);
            }
        }
    }
}

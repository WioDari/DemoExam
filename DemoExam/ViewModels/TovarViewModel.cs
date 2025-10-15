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

        public TovarViewModel()
        {
            LoadTovar();
        }

        private void LoadTovar()
        {
            var context = new AppDbContext();
            tovary = new ObservableCollection<Tovar>(context.Tovar.ToList());
            foreach (var t in tovary)
            {
                t.name = $"{t.category} | {t.name}";
                t.description = $"Описание товара: {t.description}";
                t.creator = $"Производитель: {t.creator}";
                t.supplier = $"Поставщик: {t.supplier}";
                t.photo = t.photo is null ? $"/Resources/Images/picture.png" : $"/Resources/Images/{t.photo}";
            }
        }
    }
}

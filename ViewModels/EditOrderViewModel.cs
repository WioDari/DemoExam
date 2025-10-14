using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private DateOnly _order_date;
        public DateOnly order_date
        {
            get => _order_date;
            set
            {
                _order_date = value;
                OnPropertyChanged();
            }
        }

        private DateOnly _ship_date;
        public DateOnly ship_date
        {
            get => _ship_date;
            set
            {
                _ship_date = value;
                OnPropertyChanged();
            }
        }

        public EditOrderViewModel(string? order_id = null)
        {

        }
    }
}

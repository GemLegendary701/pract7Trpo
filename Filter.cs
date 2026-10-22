using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace pract7Borisov
{
    public class Filter : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; set; } = new();
        private string _category = "";
        public string Category { get { return _category; } set { _category = value; OnPropertyChanged(); } }
        private string _minPrice = "";
        public string MinPrice { get { return _minPrice; } set { _minPrice = value; OnPropertyChanged(); } }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

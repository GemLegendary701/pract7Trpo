using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace pract7Borisov
{
    public class Product : INotifyPropertyChanged
    {
        private int _id;
        public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }

        private string _name = null!;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        private decimal _price;
        public decimal Price { get => _price; set { _price = value; OnPropertyChanged(); } }

        private int _quantity;
        public int Quantity { get => _quantity; set { _quantity = value; OnPropertyChanged(); } }

        private string _category = null!;
        public string Category { get => _category; set { _category = value; OnPropertyChanged(); } }

        private bool _isAvailable;
        public bool IsAvailable { get => _isAvailable; set { _isAvailable = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
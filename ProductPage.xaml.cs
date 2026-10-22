using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace pract7Borisov
{
    public partial class ProductPage : Page
    {
        public Product SaveProduct { get; set; }
        public static Product LastSavedProduct { get; set; }
        public static bool IsSaved { get; set; }
        public ProductPage()
        {
            InitializeComponent();
            SaveProduct = new Product();
            this.DataContext = SaveProduct;
        }
        public ProductPage(Product productToEdit)
        {
            InitializeComponent();
            SaveProduct = productToEdit;
            this.DataContext = SaveProduct;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SaveProduct.Name))
            {
                MessageBox.Show("Введите название");
                return;
            }
            if (SaveProduct.Price < 0)
            {
                MessageBox.Show("Цена не может быть меньше 0");
                return;
            }
            LastSavedProduct = SaveProduct;
            IsSaved = true;
            if (NavigationService != null)
            {
                NavigationService.GoBack();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsSaved = false;
            if (NavigationService != null)
            {
                NavigationService.GoBack();
            }
        }
    }
}
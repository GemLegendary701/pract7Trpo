using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace pract7Borisov
{
    public partial class MainWindow : Window
    {
        private ApiService _apiService = new ApiService();
        private Filter _filter;

        public MainWindow()
        {
            InitializeComponent();
            _filter = (Filter)this.DataContext;
            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                string? category = string.IsNullOrWhiteSpace(_filter.Category) ? null : _filter.Category;
                decimal? minPrice = decimal.TryParse(_filter.MinPrice, out var minPriceValue) ? minPriceValue : null;
                var products = await _apiService.GetProductsAsync(category, minPrice);

                _filter.Products.Clear();
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        _filter.Products.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения: " + ex.Message);
            }
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_filter.MinPrice) && !decimal.TryParse(_filter.MinPrice, out _))
            {
                MessageBox.Show("В поле мин цена нужно ввести число");
                return;
            }
            await LoadDataAsync();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridProducts.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар из списка");
                return;
            }
            Product selectProduct = (Product)DataGridProducts.SelectedItem;
            MainFrame.Navigate(new ProductPage(selectProduct));
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductPage());
        }

        private async void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is not ProductPage && ProductPage.IsSaved)
            {
                if (ProductPage.LastSavedProduct.Id == 0)
                {
                    // Создание (POST)
                    var response = await _apiService._client.PostAsJsonAsync("api/v1/products", ProductPage.LastSavedProduct);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Товар создан!");
                        await LoadDataAsync();
                    }
                }
                else
                {
                    var response = await _apiService._client.PutAsJsonAsync($"api/v1/products/{ProductPage.LastSavedProduct.Id}", ProductPage.LastSavedProduct);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Товар изменен!");
                        await LoadDataAsync();
                    }
                }
                ProductPage.IsSaved = false;
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridProducts.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар для удаления");
                return;
            }
            Product selectProduct = (Product)DataGridProducts.SelectedItem;

            var result = MessageBox.Show("Вы уверены, что хотите удалить " + selectProduct.Name + "?", "Подтверждение", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var response = await _apiService._client.DeleteAsync($"api/v1/products/{selectProduct.Id}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Товар удален");
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show("Ошибка сервера при удалении: " + response.StatusCode);
                }
            }
        }

        private async void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _filter.Category = "";
            _filter.MinPrice = "";
            await LoadDataAsync();
        }

        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }
    }
}
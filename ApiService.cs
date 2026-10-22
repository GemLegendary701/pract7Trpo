using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace pract7Borisov
{
    public class ApiService
    {
       public
            HttpClient _client;
        public ApiService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7196/");
        }
        public async Task<List<Product>?> GetProductsAsync(string? category, decimal? minPrice)
        {
            if (string.IsNullOrEmpty(category) && !minPrice.HasValue)
            {
                return await _client.GetFromJsonAsync<List<Product>>("api/v1/products");
            }
            string url = "api/v1/products?";
            if (!string.IsNullOrEmpty(category))
            {
                url += $"category={category}";
            }
            if (!string.IsNullOrWhiteSpace(category) && minPrice.HasValue)
            {
                url += "&";
            }
            if (minPrice.HasValue)
            {
                url += $"minPrice={minPrice}";
            }
            var responce = await _client.GetFromJsonAsync<List<Product>>(url);
            return responce;
        }
        public async Task<bool> CreateProductAsync(Product product)
        {
            var response = await _client.PostAsJsonAsync("api/v1/products", product);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateProductAsync(Product product)
        {
            var response = await _client.PutAsJsonAsync($"api/v1/products/{product.Id}", product);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _client.DeleteAsync($"api/v1/products/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}

using Newtonsoft.Json;
using PinewoodCustomerStore.UI.Models;
using System.Text;

namespace PinewoodCustomerStore.UI.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all customers
        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            var response = await _httpClient.GetAsync("api/customer");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Customer>>(content);
        }

        // Get a customer by Id
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/customer/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Customer>(content);
        }

        // Add a new customer
        public async Task AddCustomerAsync(Customer customer)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/customer", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        // Update an existing customer
        public async Task UpdateCustomerAsync(Customer customer)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/customer/{customer.Id}", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        // Delete a customer
        public async Task DeleteCustomerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/customer/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

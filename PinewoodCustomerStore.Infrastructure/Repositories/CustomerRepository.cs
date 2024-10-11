using Microsoft.EntityFrameworkCore;
using PinewoodCustomerStore.Domain.Entities;
using PinewoodCustomerStore.Domain.Interfaces;
using PinewoodCustomerStore.Infrastructure.Persistence;

namespace PinewoodCustomerStore.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly PinewoodCustomerStoreContext _context; 

        public CustomerRepository(PinewoodCustomerStoreContext context)
        {
            _context = context;
        }

        // Get all customers from the database
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync(); 
        }

        // Get customer by ID
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id); 
        }

        // Add new customer to the database
        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);  
            await _context.SaveChangesAsync();           
        }

        // Update existing customer
        public async Task UpdateCustomerAsync(Customer customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(customer.Id);
            if (existingCustomer != null)
            {
                _context.Entry(existingCustomer).CurrentValues.SetValues(customer); // Update values
                await _context.SaveChangesAsync(); // Save updated entity
            }
        }

        // Delete customer by ID
        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id); 
            if (customer != null)
            {
                _context.Customers.Remove(customer);  
                await _context.SaveChangesAsync();   
            }
        }
    }
}

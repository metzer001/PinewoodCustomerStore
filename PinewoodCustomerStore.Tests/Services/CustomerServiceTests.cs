using Moq;
using PinewoodCustomerStore.Application.Services;
using PinewoodCustomerStore.Domain.Entities;
using PinewoodCustomerStore.Domain.Interfaces;

namespace PinewoodCustomerStore.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_mockCustomerRepository.Object);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            var expectedCustomers = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "John", LastName = "Doe" },
                new Customer { Id = 2, FirstName = "Jane", LastName = "Doe" }
            };

            _mockCustomerRepository
                .Setup(repo => repo.GetAllCustomersAsync())
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerService.GetAllCustomersAsync();

            // Assert
            Assert.Equal(expectedCustomers.Count, result.Count());
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = 1;
            var expectedCustomer = new Customer { Id = customerId, FirstName = "John", LastName = "Doe" };

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByIdAsync(customerId))
                .ReturnsAsync(expectedCustomer);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCustomer.Id, result.Id);
            Assert.Equal(expectedCustomer.FirstName, result.FirstName);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = 555;

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByIdAsync(customerId))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(customerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddCustomerAsync_ShouldAddCustomer()
        {
            // Arrange
            var newCustomer = new Customer { Id = 1, FirstName = "John", LastName = "Doe" };

            // Act
            await _customerService.AddCustomerAsync(newCustomer);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.AddCustomerAsync(newCustomer), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldUpdateCustomer()
        {
            // Arrange
            var existingCustomer = new Customer { Id = 1, FirstName = "John", LastName = "Doe" };

            // Act
            await _customerService.UpdateCustomerAsync(existingCustomer);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.UpdateCustomerAsync(existingCustomer), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldDeleteCustomer()
        {
            // Arrange
            var customerId = 1;

            // Act
            await _customerService.DeleteCustomerAsync(customerId);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.DeleteCustomerAsync(customerId), Times.Once);
        }
    }
}

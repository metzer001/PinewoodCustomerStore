using Microsoft.AspNetCore.Mvc;
using Moq;
using PinewoodCustomerStore.Application.Services;
using PinewoodCustomerStore.Domain.Entities;
using PinewoodCustomerStore.Domain.Interfaces;
using PinewoodCustomerStore.API.Controllers;

namespace PinewoodCustomerStore.Tests.Controllers
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly CustomerService _customerService;
        private readonly CustomerController _controller;

        public CustomersControllerTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_mockCustomerRepository.Object);
            _controller = new CustomerController(_customerService);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsOkResult_WithCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = 1;
            var expectedCustomer = new Customer { Id = customerId, FirstName = "John", LastName = "Doe" };

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByIdAsync(customerId))
                .ReturnsAsync(expectedCustomer);

            // Act
            var result = await _controller.GetCustomer(customerId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Customer>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(expectedCustomer.Id, actualCustomer.Id);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            var customerId = 555;

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByIdAsync(customerId))
                .ReturnsAsync((Customer)null);

            var result = await _controller.GetCustomer(customerId);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task AddCustomer_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidCustomer = new Customer { FirstName = "", LastName = "" }; 
            _controller.ModelState.AddModelError("FirstName", "First Name is required");
            _controller.ModelState.AddModelError("LastName", "Last Name is required");

            // Act
            var result = await _controller.AddCustomer(invalidCustomer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task AddCustomer_ReturnsCreatedAtAction_WhenModelStateIsValid()
        {
            // Arrange
            var newCustomer = new Customer { Id = 1, FirstName = "John", LastName = "Doe" };

            _mockCustomerRepository
                .Setup(repo => repo.AddCustomerAsync(newCustomer))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddCustomer(newCustomer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedCustomer = Assert.IsType<Customer>(createdAtActionResult.Value);
            Assert.Equal(newCustomer.Id, returnedCustomer.Id);
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsBadRequest_WhenIdInUrlAndBodyDoNotMatch()
        {
            // Arrange
            var customerIdInUrl = 1;
            var customerInBody = new Customer { Id = 2, FirstName = "John", LastName = "Doe" };

            // Act
            var result = await _controller.UpdateCustomer(customerIdInUrl, customerInBody);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Customer ID in the URL does not match the ID in the body", ((dynamic)badRequestResult.Value).Message);
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            var customerId = 1;
            var customerToUpdate = new Customer { Id = customerId, FirstName = "John", LastName = "Doe" };

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByIdAsync(customerId))
                .ReturnsAsync((Customer)null);

            var result = await _controller.UpdateCustomer(customerId, customerToUpdate);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Customer with ID {customerId} not found", ((dynamic)notFoundResult.Value).Message);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            var customerId = 555; 

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByIdAsync(customerId))
                .ReturnsAsync((Customer)null);

            var result = await _controller.DeleteCustomer(customerId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Customer with ID {customerId} not found", ((dynamic)notFoundResult.Value).Message);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNoContent_WhenCustomerIsDeleted()
        {
            var customerId = 1;
            var existingCustomer = new Customer { Id = customerId, FirstName = "John", LastName = "Doe" };

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByIdAsync(customerId))
                .ReturnsAsync(existingCustomer);

            _mockCustomerRepository
                .Setup(repo => repo.DeleteCustomerAsync(customerId))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteCustomer(customerId);

            Assert.IsType<NoContentResult>(result);
        }
    }
}

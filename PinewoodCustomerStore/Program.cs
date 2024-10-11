using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinewoodCustomerStore.Application.Services;
using PinewoodCustomerStore.Domain.Interfaces;
using PinewoodCustomerStore.Infrastructure.Persistence;
using PinewoodCustomerStore.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddControllers();

builder.Services.AddDbContext<PinewoodCustomerStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Customizing API behavior for validation responses
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(e => e.Value.Errors.Select(err => err.ErrorMessage))
            .ToArray();

        var errorResponse = new
        {
            Message = "Validation Failed",
            Errors = errors
        };

        return new BadRequestObjectResult(errorResponse);
    };
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

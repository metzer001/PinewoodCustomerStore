using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PinewoodCustomerStore.Infrastructure.Persistence
{
    public class PinewoodCustomerStoreContextFactory : IDesignTimeDbContextFactory<PinewoodCustomerStoreContext>
    {
        public PinewoodCustomerStoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PinewoodCustomerStoreContext>();

            // Temporarily hardcoding the connection string for testing migrations
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=PinewoodCustomerStore;Trusted_Connection=True;TrustServerCertificate=true;";

            optionsBuilder.UseSqlServer(connectionString);

            return new PinewoodCustomerStoreContext(optionsBuilder.Options);
        }
    }
}

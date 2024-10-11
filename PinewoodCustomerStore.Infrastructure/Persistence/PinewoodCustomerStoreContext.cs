using Microsoft.EntityFrameworkCore;
using PinewoodCustomerStore.Domain.Entities;

namespace PinewoodCustomerStore.Infrastructure.Persistence
{
    public class PinewoodCustomerStoreContext : DbContext
    {
        public PinewoodCustomerStoreContext(DbContextOptions<PinewoodCustomerStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers"); 

                entity.HasKey(c => c.Id); 

                entity.Property(c => c.FirstName)
                      .IsRequired() 
                      .HasMaxLength(50); 

                entity.Property(c => c.LastName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(c => c.Email)
                      .IsRequired()
                      .HasMaxLength(100);
                
                entity.HasIndex(c => c.Email).IsUnique();

               
            });
        }
    }
}

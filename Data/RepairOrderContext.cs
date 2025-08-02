using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Data
{
    public class RepairOrderContext : DbContext
    {
        public RepairOrderContext(DbContextOptions<RepairOrderContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RepairOrder> RepairOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);

                // Configure one-to-many relationship
                entity.HasMany(e => e.RepairOrders)
                    .WithOne(e => e.Customer)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Vehicle entity
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Make).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Model).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Year).IsRequired();

                // Configure one-to-many relationship
                entity.HasMany(e => e.RepairOrders)
                    .WithOne(e => e.Vehicle)
                    .HasForeignKey(e => e.VehicleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure RepairOrder entity
            modelBuilder.Entity<RepairOrder>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.EstimatedCost).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Open");
                entity.Property(e => e.CreatedDate).IsRequired();

                // Configure foreign key relationships
                entity.HasOne(e => e.Customer)
                    .WithMany(e => e.RepairOrders)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Vehicle)
                    .WithMany(e => e.RepairOrders)
                    .HasForeignKey(e => e.VehicleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, FirstName = "John", LastName = "Smith", PhoneNumber = "555-0123" },
                new Customer { Id = 2, FirstName = "Jane", LastName = "Johnson", PhoneNumber = "555-0456" },
                new Customer { Id = 3, FirstName = "Bob", LastName = "Wilson", PhoneNumber = "555-0789" },
                new Customer { Id = 4, FirstName = "Alice", LastName = "Brown", PhoneNumber = "555-0321" },
                new Customer { Id = 5, FirstName = "Mike", LastName = "Davis", PhoneNumber = "555-0654" }
            );

            // Seed vehicles
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { Id = 1, Year = 2020, Make = "Toyota", Model = "Camry" },
                new Vehicle { Id = 2, Year = 2019, Make = "Honda", Model = "Civic" },
                new Vehicle { Id = 3, Year = 2021, Make = "Ford", Model = "F-150" },
                new Vehicle { Id = 4, Year = 2018, Make = "Chevrolet", Model = "Malibu" },
                new Vehicle { Id = 5, Year = 2022, Make = "BMW", Model = "X3" },
                new Vehicle { Id = 6, Year = 2020, Make = "Audi", Model = "A4" }
            );

            // Seed repair orders
            modelBuilder.Entity<RepairOrder>().HasData(
                new RepairOrder { Id = 1, CustomerId = 1, VehicleId = 1, CreatedDate = DateTime.Now.AddDays(-15), Description = "Oil change and brake inspection", EstimatedCost = 150.00m, Status = "Completed" },
                new RepairOrder { Id = 2, CustomerId = 2, VehicleId = 2, CreatedDate = DateTime.Now.AddDays(-10), Description = "Transmission repair", EstimatedCost = 2500.00m, Status = "In Progress" },
                new RepairOrder { Id = 3, CustomerId = 1, VehicleId = 1, CreatedDate = DateTime.Now.AddDays(-5), Description = "Replace air filter and spark plugs", EstimatedCost = 200.00m, Status = "Open" },
                new RepairOrder { Id = 4, CustomerId = 3, VehicleId = 3, CreatedDate = DateTime.Now.AddDays(-3), Description = "Tire rotation and alignment", EstimatedCost = 120.00m, Status = "Completed" },
                new RepairOrder { Id = 5, CustomerId = 4, VehicleId = 4, CreatedDate = DateTime.Now.AddDays(-7), Description = "Engine diagnostic and repair", EstimatedCost = 800.00m, Status = "In Progress" },
                new RepairOrder { Id = 6, CustomerId = 2, VehicleId = 2, CreatedDate = DateTime.Now.AddDays(-2), Description = "Battery replacement", EstimatedCost = 180.00m, Status = "Open" },
                new RepairOrder { Id = 7, CustomerId = 5, VehicleId = 5, CreatedDate = DateTime.Now.AddDays(-1), Description = "Annual maintenance service", EstimatedCost = 450.00m, Status = "Open" }
            );
        }
    }
}
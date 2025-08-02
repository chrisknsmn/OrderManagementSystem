using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework Core with SQL Server (production) or SQLite (development)
builder.Services.AddDbContext<RepairOrderContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    else
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
});

// Register DataService with EF Core implementation
builder.Services.AddScoped<IDataService, EfDataService>();

// Add CORS configuration for console app communication
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowConsoleApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RepairOrderContext>();
    try
    {
        context.Database.EnsureCreated();
        
        // Add initial repair orders if they don't exist
        if (!context.RepairOrders.Any())
        {
            var repairOrders = new[]
            {
                new RepairOrder { Id = 1, CustomerId = 1, VehicleId = 1, CreatedDate = DateTime.Now.AddDays(-15), Description = "Oil change and brake inspection", EstimatedCost = 150.00m, Status = "Completed" },
                new RepairOrder { Id = 2, CustomerId = 2, VehicleId = 2, CreatedDate = DateTime.Now.AddDays(-10), Description = "Transmission repair", EstimatedCost = 2500.00m, Status = "In Progress" },
                new RepairOrder { Id = 3, CustomerId = 1, VehicleId = 1, CreatedDate = DateTime.Now.AddDays(-5), Description = "Replace air filter and spark plugs", EstimatedCost = 200.00m, Status = "Open" },
                new RepairOrder { Id = 4, CustomerId = 3, VehicleId = 3, CreatedDate = DateTime.Now.AddDays(-3), Description = "Tire rotation and alignment", EstimatedCost = 120.00m, Status = "Completed" },
                new RepairOrder { Id = 5, CustomerId = 4, VehicleId = 4, CreatedDate = DateTime.Now.AddDays(-7), Description = "Engine diagnostic and repair", EstimatedCost = 800.00m, Status = "In Progress" },
                new RepairOrder { Id = 6, CustomerId = 2, VehicleId = 2, CreatedDate = DateTime.Now.AddDays(-2), Description = "Battery replacement", EstimatedCost = 180.00m, Status = "Open" },
                new RepairOrder { Id = 7, CustomerId = 5, VehicleId = 5, CreatedDate = DateTime.Now.AddDays(-1), Description = "Annual maintenance service", EstimatedCost = 450.00m, Status = "Open" }
            };
            
            context.RepairOrders.AddRange(repairOrders);
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // Log error but don't crash the application
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating/seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS before other middleware
app.UseCors("AllowConsoleApp");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
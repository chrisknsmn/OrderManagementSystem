using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

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
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        // Ensure database is created
        var created = context.Database.EnsureCreated();
        logger.LogInformation($"Database created: {created}");
        
        // Check if data already exists
        var customerCount = context.Customers.Count();
        var vehicleCount = context.Vehicles.Count();
        var orderCount = context.RepairOrders.Count();
        
        logger.LogInformation($"Existing data - Customers: {customerCount}, Vehicles: {vehicleCount}, Orders: {orderCount}");
        
        // If no data exists, the model seeding should have handled it
        // But let's add some additional orders with current dates for better demo
        if (orderCount < 5)
        {
            logger.LogInformation("Adding additional demo repair orders...");
            
            var additionalOrders = new[]
            {
                new RepairOrder { CustomerId = 1, VehicleId = 1, CreatedDate = DateTime.Now.AddDays(-15), Description = "Oil change and brake inspection", EstimatedCost = 150.00m, Status = "Completed" },
                new RepairOrder { CustomerId = 2, VehicleId = 2, CreatedDate = DateTime.Now.AddDays(-10), Description = "Transmission repair", EstimatedCost = 2500.00m, Status = "In Progress" },
                new RepairOrder { CustomerId = 1, VehicleId = 1, CreatedDate = DateTime.Now.AddDays(-5), Description = "Replace air filter and spark plugs", EstimatedCost = 200.00m, Status = "Open" },
                new RepairOrder { CustomerId = 3, VehicleId = 3, CreatedDate = DateTime.Now.AddDays(-3), Description = "Tire rotation and alignment", EstimatedCost = 120.00m, Status = "Completed" },
                new RepairOrder { CustomerId = 4, VehicleId = 4, CreatedDate = DateTime.Now.AddDays(-7), Description = "Engine diagnostic and repair", EstimatedCost = 800.00m, Status = "In Progress" },
                new RepairOrder { CustomerId = 2, VehicleId = 2, CreatedDate = DateTime.Now.AddDays(-2), Description = "Battery replacement", EstimatedCost = 180.00m, Status = "Open" }
            };
            
            context.RepairOrders.AddRange(additionalOrders);
            var savedCount = context.SaveChanges();
            logger.LogInformation($"Added {savedCount} repair orders to database");
        }
        
        // Log final counts
        var finalOrderCount = context.RepairOrders.Count();
        logger.LogInformation($"Final repair order count: {finalOrderCount}");
    }
    catch (Exception ex)
    {
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

// Serve static files from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

// Fallback to index.html for SPA routing
app.MapFallbackToFile("index.html");

app.Run();
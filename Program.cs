using OrderManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add DataService as singleton (for in-memory data)
builder.Services.AddSingleton<DataService>();

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
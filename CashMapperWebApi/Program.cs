// Create app builder.
using CashMapper.DataAccess;
using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using Serilog;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);


// Find current dir.
var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName + "//CashMapperWebApi//";

// Enable logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(projectDirectory + "/log.txt")
    .CreateLogger();
builder.Host.UseSerilog();


// Create config for appsettings.json.
var config = new ConfigurationBuilder()
    .SetBasePath(projectDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Register backend services.
builder.Services
    .AddSingleton<IConfiguration>(config)
    .Configure<DatabaseSettings>(config.GetSection("DatabaseSettings"))
    .AddSingleton<IDatabaseFactory, DatabaseFactory>()
    .AddSingleton<IRepository<Category>, CategoryRepository>()
    .AddSingleton<IRepository<IncomeItem>, IncomeItemRepository>()
    .AddSingleton<IRepository<BudgetItem>, BudgetItemRepository>()
    .AddSingleton<IRepository<ExpenseItem>, ExpenseItemRepository>()
    .AddSingleton<IRepository<Transaction>, TransactionRepository>()
    .AddSingleton<IRepository<IncomeProfile>, IncomeProfileRepository>()
    .AddSingleton<IRepository<AccountProfile>, AccountProfileRepository>()

    // Enable CORS
    .AddCors(options =>
    {
        options.AddPolicy("AllowReactApp", policy =>
        {
            policy.WithOrigins("http://localhost:5175") // React app port
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the app.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowReactApp");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

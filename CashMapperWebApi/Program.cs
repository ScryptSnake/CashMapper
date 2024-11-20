// Create app builder.
using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using CashMapper.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Create config for appsettings.json.
var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName + "//CashMapperWebApi//";

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
    .AddSingleton<IRepository<AccountProfile>, AccountProfileRepository>(); ;

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
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

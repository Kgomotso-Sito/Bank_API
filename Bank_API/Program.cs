using BankAPI.Repositories;
using BankAPI.Services;
using BankAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAccountHolderRepository, AccountHolderRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IAccountHolderService, AccountHolderService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();


builder.Services.AddControllers();
builder.Services.AddDbContext<BankAccountDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BankAccountDb"));
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Call Migrate() method to apply migrations at runtime
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<BankAccountDbContext>();
context.Database.Migrate();

app.Run();


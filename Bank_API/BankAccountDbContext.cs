using BankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAPI;

public class BankAccountDbContext : DbContext
{
    public BankAccountDbContext()
    {

    }
    public BankAccountDbContext(DbContextOptions<BankAccountDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>()
        .Property(b => b.AvailableBalance)
            .HasColumnType("decimal(18,2)");
    }

    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<AccountHolder> AccountHolders { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
}

using BankAPI.Models;
using BankAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace BankAPI.Tests.Repositories;

public class AccountHolderRepositoryTests
{
    private readonly DbContextOptions<BankAccountDbContext> _options;

    public AccountHolderRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<BankAccountDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetAccountHolderByIdNumber_ReturnsAccountHolder_WhenFound()
    {
        using (var context = new BankAccountDbContext(_options))
        {
            var accountHolder = new AccountHolder
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1),
                IdNumber = "8001010001",
                ResidentialAddress = "123 Main St",
                MobileNumber = "555-1234",
                EmailAddress = "john.doe@example.com"
            };
            context.AccountHolders.Add(accountHolder);
            await context.SaveChangesAsync();
        }

        using (var context = new BankAccountDbContext(_options))
        {
            var repository = new AccountHolderRepository(context);

            var result = await repository.GetAccountHolderByIdNumber("8001010001");

            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal(new DateTime(1980, 1, 1), result.DateOfBirth);
            Assert.Equal("8001010001", result.IdNumber);
            Assert.Equal("123 Main St", result.ResidentialAddress);
            Assert.Equal("555-1234", result.MobileNumber);
            Assert.Equal("john.doe@example.com", result.EmailAddress);
        }
    }

    [Fact]
    public async Task GetAccountHolderByIdNumber_ReturnsNull_WhenNotFound()
    {
        using (var context = new BankAccountDbContext(_options))
        {
            var repository = new AccountHolderRepository(context);

            var result = await repository.GetAccountHolderByIdNumber("8001011001");

            Assert.Null(result);
        }
    }
}

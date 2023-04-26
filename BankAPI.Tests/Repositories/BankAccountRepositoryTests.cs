using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAPI.Models;
using BankAPI.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BankAPI.Tests.Repositories;

public class BankAccountRepositoryTests
{
    private readonly DbContextOptions<BankAccountDbContext> _options;

    public BankAccountRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<BankAccountDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetBankAccountsByAccountHolderId_ReturnsCorrectBankAccounts()
    {
        var accountHolderId = 1;

        using (var context = new BankAccountDbContext(_options))
        {
            var accountHolder = new AccountHolder
            {
                Id = accountHolderId,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1),
                IdNumber = "1234567890",
                ResidentialAddress = "123 Main St, Anytown",
                MobileNumber = "555-1234",
                EmailAddress = "john.doe@example.com",
                BankAccounts = new List<BankAccount>
                {
                    new BankAccount
                    {
                        Id = 1,
                        AccountNumber = "1234567890",
                        AccountType = AccountType.Cheque,
                        Name = "John's Cheque Account",
                        AccountStatus = AccountStatus.Active,
                        AvailableBalance = 1000.00M
                    },
                    new BankAccount
                    {
                        Id = 2,
                        AccountNumber = "0987654321",
                        AccountType = AccountType.Savings,
                        Name = "John's Savings Account",
                        AccountStatus = AccountStatus.Inactive,
                        AvailableBalance = 5000.00M
                    }
                }
            };
            context.AccountHolders.Add(accountHolder);
            await context.SaveChangesAsync();
        }

        using (var context = new BankAccountDbContext(_options))
        {
            var repository = new BankAccountRepository(context);
            var bankAccounts = await repository.GetBankAccountsByAccountHolderId(accountHolderId);
            var bankAccountIds = bankAccounts.Select(b => b.Id).ToList();

            Assert.Equal(2, bankAccounts.Count);
            Assert.Contains(1, bankAccountIds);
            Assert.Contains(2, bankAccountIds);
        }
    }

    [Fact]
    public async Task GetBankAccountsByAccountHolderId_ReturnsEmptyList_WhenAccountHolderNotFound()
    {
        var accountHolderId = 1;
        using (var context = new BankAccountDbContext(_options))
        {
            var repository = new BankAccountRepository(context);

            var bankAccounts = await repository.GetBankAccountsByAccountHolderId(accountHolderId);

            Assert.Empty(bankAccounts);
        }
    }

    [Fact]
    public async Task GetBankAccountByAccountNumber_ReturnsBankAccount_WhenAccountNumberExists()
    {
        using (var context = new BankAccountDbContext(_options))
        {
            var bankAccount = new BankAccount
            {
                Id = 1,
                AccountNumber = "1234567890",
                AccountType = AccountType.Cheque,
                Name = "Test Account",
                AccountStatus = AccountStatus.Active,
                AvailableBalance = 1000
            };
            context.BankAccounts.Add(bankAccount);
            await context.SaveChangesAsync();
        }

        using (var context = new BankAccountDbContext(_options))
        {
            var repository = new BankAccountRepository(context);

            var result = await repository.GetBankAccountByAccountNumber("1234567890");

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("1234567890", result.AccountNumber);
            Assert.Equal("Test Account", result.Name);
            Assert.Equal(AccountType.Cheque, result.AccountType);
            Assert.Equal(AccountStatus.Active, result.AccountStatus);
            Assert.Equal(1000, result.AvailableBalance);
        }
    }

    [Fact]
    public async Task GetBankAccountByAccountNumber_ReturnsNull_WhenAccountNumberNotFound()
    {
        using (var context = new BankAccountDbContext(_options))
        {
            var repository = new BankAccountRepository(context);

            var result = await repository.GetBankAccountByAccountNumber("000000000");

            Assert.Null(result);
        }
    }

    [Fact]
    public void UpdateBankAccount_UpdatesBankAccountInDatabaseAsync()
    {
        using (var context = new BankAccountDbContext(_options))
        {
            var repository = new BankAccountRepository(context);
            var bankAccount = new BankAccount
            {
                AccountNumber = "1234567890",
                AccountType = AccountType.Cheque,
                Name = "Test Account",
                AccountStatus = AccountStatus.Active,
                AvailableBalance = 100.00m
            };
            context.BankAccounts.Add(bankAccount);
            context.SaveChanges();
        }

        using (var context = new BankAccountDbContext(_options))
        {
            var repository = new BankAccountRepository(context);

            var bankAccountToUpdate = repository.GetBankAccountByAccountNumber("1234567890").Result;
            bankAccountToUpdate.AvailableBalance = 50.00m;

            repository.UpdateBankAccount(bankAccountToUpdate);

            var updatedBankAccount = repository.GetBankAccountByAccountNumber("1234567890").Result;
            Assert.Equal(50.00m, updatedBankAccount.AvailableBalance);
        }
    }
}


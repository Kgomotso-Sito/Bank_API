using Moq;
using Xunit;
using BankAPI.Models;
using BankAPI.Repositories;
using BankAPI.Services;

namespace BankAPI.Tests.Services;

public class BankAccountServiceTests
{
    private readonly BankAccountService _bankAccountService;
    private readonly Mock<IBankAccountRepository> _mockRepository;

    public BankAccountServiceTests()
    {
        _mockRepository = new Mock<IBankAccountRepository>();
        _bankAccountService = new BankAccountService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetBankAccountsByAccountHolderId_WithValidId_ReturnsBankAccounts()
    {
        int accountHolderId = 1;
        var expectedBankAccounts = new List<BankAccount>()
        {
            new BankAccount()
            {
                Id = 1, AccountNumber = "12345678",
                AccountType = AccountType.Cheque, Name = "Cheque Account",
                AccountStatus = AccountStatus.Active, AvailableBalance = 1000
            },
            new BankAccount()
            {
                Id = 2, AccountNumber = "23456789",
                AccountType = AccountType.Savings, Name = "Savings Account",
                AccountStatus = AccountStatus.Active, AvailableBalance = 2000
            }
        };

        _mockRepository.Setup(repo => repo.GetBankAccountsByAccountHolderId(accountHolderId)).ReturnsAsync(expectedBankAccounts);

        var actualBankAccounts = await _bankAccountService.GetBankAccountsByAccountHolderId(accountHolderId);

        Assert.Equal(expectedBankAccounts, actualBankAccounts);
    }

    [Fact]
    public async Task GetBankAccountsByAccountHolderId_WithInvalidId_ReturnsEmptyList()
    {
        int accountHolderId = 1;
        _mockRepository.Setup(repo => repo.GetBankAccountsByAccountHolderId(accountHolderId)).ReturnsAsync(new List<BankAccount>());

        var actualBankAccounts = await _bankAccountService.GetBankAccountsByAccountHolderId(accountHolderId);

        Assert.Empty(actualBankAccounts);
    }
}

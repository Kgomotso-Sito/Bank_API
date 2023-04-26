using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BankAPI.Controllers;
using BankAPI.Services;
using BankAPI.Models;
using Microsoft.Extensions.Logging;

namespace BankAPI.Tests.Controllers;

public class BankAccountControllerTests
{
    private readonly BankAccountController _bankAccountController;
    private readonly Mock<ILogger<BankAccountController>> _logger;
    private readonly Mock<IAccountHolderService> _accountHolderService;
    private readonly Mock<IBankAccountService> _bankAccountService;

    public BankAccountControllerTests()
    {
        _logger = new Mock<ILogger<BankAccountController>>();
        _accountHolderService = new Mock<IAccountHolderService>();
        _bankAccountService = new Mock<IBankAccountService>();
        _bankAccountController = new BankAccountController(_logger.Object, _accountHolderService.Object, _bankAccountService.Object);
    }


    [Fact]
    public async Task GetBankAccountByAccountNumber_ReturnsBadRequest_WhenAccountNumberIsInvalid()
    {
        string accountNumber = "AA1234567890";
        string expectedErrorMessage = "Account number is invalid";

        IActionResult result = await _bankAccountController.GetBankAccountByAccountNumber(accountNumber);

        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        string errorMessage = Assert.IsType<string>(badRequestResult.Value);
        Assert.Equal(expectedErrorMessage, errorMessage);
    }

    [Fact]
    public async void GetBankAccountByAccountNumber_ReturnsBadRequest_WhenNoAccountFoundAsync()
    {
        string accountNumber = "1234567890";
        BankAccount bankAccount = null;
        _bankAccountService.Setup(s => s.GetBankAccountByAccountNumber(accountNumber)).ReturnsAsync(bankAccount);

        IActionResult result = await _bankAccountController.GetBankAccountByAccountNumber(accountNumber);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("No Account found", (result as BadRequestObjectResult).Value);
    }

    [Fact]
    public async void GetBankAccountByAccountNumber_ReturnsOkResult_WhenAccountFound()
    {
        string accountNumber = "1234567890";
        BankAccount bankAccount = new BankAccount { AccountNumber = accountNumber };
        _bankAccountService.Setup(s => s.GetBankAccountByAccountNumber(accountNumber)).ReturnsAsync(bankAccount);

        IActionResult result = await _bankAccountController.GetBankAccountByAccountNumber(accountNumber);

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(bankAccount, (result as OkObjectResult).Value);
    }

    [Fact]
    public async Task GetBankAccountsByIdNumber_ReturnsBadRequest_WhenNoAccountsFound()
    {
        string validIdNumber = "1234567890123";
        AccountHolder nullAccountHolder = null;
        IEnumerable<BankAccount> emptyBankAccounts = Enumerable.Empty<BankAccount>();
        _accountHolderService.Setup(s => s.GetAccountHolderByIdNumber(validIdNumber)).ReturnsAsync(nullAccountHolder);
        _bankAccountService.Setup(s => s.GetBankAccountsByAccountHolderId(It.IsAny<int>())).ReturnsAsync(emptyBankAccounts);
        string expectedErrorMessage = "Accounts not found";

        IActionResult result = await _bankAccountController.GetBankAccountsByIdNumber(validIdNumber);

        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        string errorMessage = Assert.IsType<string>(badRequestResult.Value);
        Assert.Equal(expectedErrorMessage, errorMessage);
    }

    [Fact]
    public async Task GetBankAccountsByIdNumber_ReturnsOk_WhenAccountsFound()
    {
        string validIdNumber = "1234567890123"; 
        AccountHolder accountHolder = new AccountHolder { Id = 1 };
        IEnumerable<BankAccount> bankAccounts = new List<BankAccount> { new BankAccount { Id = 1, AccountNumber = "123", AccountType = AccountType.Cheque } };
        _accountHolderService.Setup(s => s.GetAccountHolderByIdNumber(validIdNumber)).ReturnsAsync(accountHolder);
        _bankAccountService.Setup(s => s.GetBankAccountsByAccountHolderId(accountHolder.Id)).ReturnsAsync(bankAccounts);

        IActionResult result = await _bankAccountController.GetBankAccountsByIdNumber(validIdNumber);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        IEnumerable<BankAccount> returnedBankAccounts = Assert.IsAssignableFrom<IEnumerable<BankAccount>>(okResult.Value);
        Assert.Equal(bankAccounts.Count(), returnedBankAccounts.Count());
    }
}

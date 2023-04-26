using Moq;
using Xunit;
using BankAPI.Models;
using BankAPI.Repositories;
using BankAPI.Services;

namespace BankAPI.Tests.Services;

public class AccountHolderServiceTests
{
    private readonly AccountHolderService _accountHolderService;
    private readonly Mock<IAccountHolderRepository> _mockRepository;

    public AccountHolderServiceTests()
    {
        _mockRepository = new Mock<IAccountHolderRepository>();
        _accountHolderService = new AccountHolderService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAccountHolderByIdNumber_ReturnsAccountHolder()
    {
        var idNumber = "123456789";
        var expectedAccountHolder = new AccountHolder
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

        _mockRepository.Setup(repo => repo.GetAccountHolderByIdNumber(idNumber))
            .ReturnsAsync(expectedAccountHolder);

        var result = await _accountHolderService.GetAccountHolderByIdNumber(idNumber);

        Assert.NotNull(result);
        Assert.IsType<AccountHolder>(result);
        Assert.Equal(expectedAccountHolder, result);
    }

    [Fact]
    public async Task GetAccountHolderByIdNumber_ReturnsNullForNonexistentIdNumber()
    {
        var idNumber = "123456789";

        var result = await _accountHolderService.GetAccountHolderByIdNumber(idNumber);

        Assert.Null(result);
    }
}

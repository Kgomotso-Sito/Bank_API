using BankAPI.Models;
using BankAPI.Repositories;
using BankAPI.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BankAPI.Tests.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<IBankAccountRepository> _mockBankRepository;
        private readonly Mock<IAuditLogRepository> _mockAuditLogRepository;

        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _mockBankRepository = new Mock<IBankAccountRepository>();
            _mockAuditLogRepository = new Mock<IAuditLogRepository>();
            _transactionService = new TransactionService(_mockBankRepository.Object, _mockAuditLogRepository.Object);
        }

        [Fact]
        public void Withdraw_ThrowsArgumentException_WhenBankAccountNotFound()
        {
            _mockBankRepository.Setup(repo => repo.GetBankAccountByAccountNumber("12345678"))
                .ReturnsAsync((BankAccount)null);

            Assert.Throws<ArgumentException>(() => _transactionService.Withdraw("12345678", 100));
        }

        [Fact]
        public void Withdraw_ThrowsInvalidOperationException_WhenBankAccountNotActive()
        {
            var inactiveBankAccount = new BankAccount
            {
                AccountNumber = "12345678",
                AccountStatus = AccountStatus.Inactive,
                AvailableBalance = 500
            };
            _mockBankRepository.Setup(repo => repo.GetBankAccountByAccountNumber("12345678"))
                .ReturnsAsync(inactiveBankAccount);

            Assert.Throws<InvalidOperationException>(() => _transactionService.Withdraw("12345678", 100));
        }

        [Fact]
        public void Withdraw_ThrowsArgumentException_WhenWithdrawalAmountIsZero()
        {
            var activeBankAccount = new BankAccount
            {
                AccountNumber = "12345678",
                AccountStatus = AccountStatus.Active,
                AvailableBalance = 500
            };
            _mockBankRepository.Setup(repo => repo.GetBankAccountByAccountNumber("12345678"))
                .ReturnsAsync(activeBankAccount);

            Assert.Throws<ArgumentException>(() => _transactionService.Withdraw("12345678", 0));
        }

        [Fact]
        public void Withdraw_ThrowsInvalidOperationException_WhenFixedDepositAmountNotEqualToAvailableBalance()
        {
            var bankAccount = new BankAccount
            {
                AccountNumber = "12345678",
                AccountType = AccountType.FixedDeposit,
                AvailableBalance = 500.0m,
                AccountStatus = AccountStatus.Active
            };

            _mockBankRepository.Setup(x => x.GetBankAccountByAccountNumber(It.IsAny<string>()))
                .ReturnsAsync(bankAccount);

            Assert.Throws<InvalidOperationException>(() => _transactionService.Withdraw("12345678", 1000));
        }

        [Fact]
        public void Withdraw_ThrowsInvalidOperationException_WhenInsufficientFunds()
        {
            var activeBankAccount = new BankAccount
            {
                AccountNumber = "12345678",
                AccountStatus = AccountStatus.Active,
                AvailableBalance = 500
            };
            _mockBankRepository.Setup(repo => repo.GetBankAccountByAccountNumber("12345678"))
                .ReturnsAsync(activeBankAccount);

            Assert.Throws<InvalidOperationException>(() => _transactionService.Withdraw("12345678", 1000));
        }

        [Fact]
        public void Withdraw_ThrowsInvalidOperationException_WhenAmountIsGreaterThanAvailableBalance()
        {
            var bankAccount = new BankAccount
            {
                AccountStatus = AccountStatus.Active,
                AvailableBalance = 100
            };

            var bankAccountRepository = new Mock<IBankAccountRepository>();
            bankAccountRepository.Setup(r => r.GetBankAccountByAccountNumber("12345")).ReturnsAsync(bankAccount);

            var auditTrailRepository = new Mock<IAuditLogRepository>();
            var transactionService = new TransactionService(bankAccountRepository.Object, auditTrailRepository.Object);

            Assert.Throws<InvalidOperationException>(() => transactionService.Withdraw("12345", 200));
        }

        [Fact]
        public void Withdraw_UpdatesBankAccountAndSavesAuditLog_WhenValidWithdrawal()
        {
            var accountNumber = "123456789";
            var amount = 500.00m;
            var bankAccount = new BankAccount
            {
                AccountNumber = accountNumber,
                AccountStatus = AccountStatus.Active,
                AvailableBalance = 1000.00m
            };
            var auditTrail = new AuditLog();
            
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            bankAccountRepository.Setup(x => x.GetBankAccountByAccountNumber(accountNumber)).ReturnsAsync(bankAccount);

            var auditTrailRepository = new Mock<IAuditLogRepository>();
            auditTrailRepository.Setup(x => x.SaveAuditLog(It.IsAny<AuditLog>())).Callback<AuditLog>(x => auditTrail = x);

            var transactionService = new TransactionService(bankAccountRepository.Object, auditTrailRepository.Object);

            transactionService.Withdraw(accountNumber, amount);

            Assert.Equal(500.00m, bankAccount.AvailableBalance);
            bankAccountRepository.Verify(x => x.UpdateBankAccount(bankAccount), Times.Once);
            Assert.Equal(Models.Action.Withdraw, auditTrail.Action);
            Assert.Equal(bankAccount, auditTrail.BankAccount);
            Assert.True(auditTrail.Timestamp > DateTime.MinValue);
        }  
    }
}
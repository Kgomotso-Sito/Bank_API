using System.ComponentModel.DataAnnotations.Schema;
using BankAPI.Models;
using BankAPI.Repositories;

namespace BankAPI.Services;

public class TransactionService : ITransactionService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IAuditLogRepository _auditTrailRepository;

    public TransactionService(IBankAccountRepository bankAccountRepository, IAuditLogRepository auditTrailRepository)
    {
        _bankAccountRepository = bankAccountRepository;
        _auditTrailRepository = auditTrailRepository;
    }

    public bool Withdraw(string accountNumber, decimal amount)
    {
        var bankAccount = _bankAccountRepository.GetBankAccountByAccountNumber(accountNumber).Result;

        if (bankAccount == null)
        {
            throw new ArgumentException("Bank account not found.");
        }

        if (bankAccount.AccountStatus != AccountStatus.Active)
        {
            throw new InvalidOperationException("Bank account is not active.");
        }

        if (amount <= 0)
        {
            throw new ArgumentException("Withdrawal amount must be greater than zero.");
        }

        if (amount == bankAccount.AvailableBalance && bankAccount.AccountType != AccountType.FixedDeposit)
        {
            throw new InvalidOperationException("A full available amount withdrawal is allowed on Fixed Deposit account type.");
        }

        if (amount > bankAccount.AvailableBalance)
        {
            throw new InvalidOperationException("Insufficient funds.");
        }

        bankAccount.AvailableBalance -= amount;

        _bankAccountRepository.UpdateBankAccount(bankAccount);

        var withdrawalTransaction = new AuditLog
        {
            Action = Models.Action.Withdraw,
            Timestamp = DateTime.Now,
            BankAccount = bankAccount
        };

        _auditTrailRepository.SaveAuditLog(withdrawalTransaction);

        return true;
    }
}



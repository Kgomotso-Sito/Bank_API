using BankAPI.Models;

namespace BankAPI.Services;

public interface ITransactionService
{
    bool Withdraw(string accountNumber, decimal amount);
}


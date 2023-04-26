using System.Transactions;
using BankAPI.Models;

namespace BankAPI.Repositories;

public interface IBankAccountRepository
{
    Task<List<BankAccount>> GetBankAccountsByAccountHolderId(int AccountHolderId);
    Task<BankAccount?> GetBankAccountByAccountNumber(string AccountNumber);
}



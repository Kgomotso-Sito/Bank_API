using BankAPI.Models;

namespace BankAPI.Services;

public interface IBankAccountService
{
    Task<IEnumerable<BankAccount>> GetBankAccountsByAccountHolderId(int AccountHolderId);
    Task<BankAccount?> GetBankAccountByAccountNumber(string AccountNumber);
}

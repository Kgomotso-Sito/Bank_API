using BankAPI.Models;
using BankAPI.Repositories;

namespace BankAPI.Services;

public class BankAccountService: IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountService(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<IEnumerable<BankAccount>> GetBankAccountsByAccountHolderId(int AccountHolderId)
    {
        var bankAccounts = await _bankAccountRepository.GetBankAccountsByAccountHolderId(AccountHolderId);

        return bankAccounts;
    }

    public async Task<BankAccount?> GetBankAccountByAccountNumber(string AccountNumber)
    {
        var bankAccount = await _bankAccountRepository.GetBankAccountByAccountNumber(AccountNumber);

        return bankAccount;
    }

}



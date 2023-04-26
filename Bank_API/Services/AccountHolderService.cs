using BankAPI.Models;
using BankAPI.Repositories;

namespace BankAPI.Services;

public class AccountHolderService : IAccountHolderService
{
    private readonly IAccountHolderRepository _accountHolderRepository;

    public AccountHolderService(IAccountHolderRepository accountHolderRepository)
    {
        _accountHolderRepository = accountHolderRepository;
    }

    public async Task<AccountHolder?> GetAccountHolderByIdNumber(string IdNumber)
    {
        var accountHolder = await _accountHolderRepository.GetAccountHolderByIdNumber(IdNumber);

        return accountHolder;
    }

}
using Microsoft.EntityFrameworkCore;
using BankAPI.Models;

namespace BankAPI.Repositories;

public class AccountHolderRepository : IAccountHolderRepository
{
    private readonly BankAccountDbContext _context;

    public AccountHolderRepository(BankAccountDbContext context)
    {
        _context = context;
    }

    public async Task<AccountHolder?> GetAccountHolderByIdNumber(string IdNumber)
    {
        var accountHolder = await _context.AccountHolders
        .SingleOrDefaultAsync(a => a.IdNumber == IdNumber);

        return accountHolder;
    }
}



using System.Transactions;
using Microsoft.EntityFrameworkCore;
using BankAPI.Models;

namespace BankAPI.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly BankAccountDbContext _context;

    public BankAccountRepository(BankAccountDbContext context)
    {
        _context = context;
    }

    public async Task<List<BankAccount>> GetBankAccountsByAccountHolderId(int AccountHolderId)
    {
        return await _context.BankAccounts
            .Where(b => b.AccountHolder.Id == AccountHolderId)
            .ToListAsync();
    }

    public async Task<BankAccount?> GetBankAccountByAccountNumber(string AccountNumber)
    {
        var bankAccount = await _context.BankAccounts
            .SingleOrDefaultAsync(b => b.AccountNumber == AccountNumber);

        return bankAccount;
    }

    public void UpdateBankAccount(BankAccount bankAccount)
    {
        _context.BankAccounts.Update(bankAccount);
        _context.SaveChanges();
    }

}



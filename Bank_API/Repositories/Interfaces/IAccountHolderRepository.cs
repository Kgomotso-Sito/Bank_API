using System.Transactions;
using BankAPI.Models;

namespace BankAPI.Repositories;

public interface IAccountHolderRepository
{
    Task<AccountHolder?> GetAccountHolderByIdNumber(string IdNumber);
}



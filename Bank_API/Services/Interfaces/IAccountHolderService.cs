using BankAPI.Models;

namespace BankAPI.Services;

public interface IAccountHolderService
{
 Task<AccountHolder?> GetAccountHolderByIdNumber(string IdNumber);
}

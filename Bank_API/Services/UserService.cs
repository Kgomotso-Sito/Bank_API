using BankAPI.Models;
using BankAPI.Repositories;

namespace BankAPI.Services;

public class UserService : IUserService
{
    public bool Authenticate(string Username, string Password)
    {
        return Username.Equals("bankUser") && Password.Equals("bankPassword");
    }
}
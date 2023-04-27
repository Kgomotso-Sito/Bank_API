namespace BankAPI.Services;

public interface IUserService
{
    bool Authenticate(string Username, string Password);
}


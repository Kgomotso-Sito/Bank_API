using Xunit;
using BankAPI.Services;
using System;

namespace BankAPI.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public void Authenticate_ValidCredentials_ReturnsTrue()
    {
        var userService = new UserService();

        var result = userService.Authenticate("bankUser", "bankPassword");

        Assert.True(result);
    }

    [Fact]
    public void Authenticate_InvalidCredentials_ReturnsFalse()
    {
        var userService = new UserService();

        var result = userService.Authenticate("invalidUser", "invalidPassword");

        Assert.False(result);
    }
}

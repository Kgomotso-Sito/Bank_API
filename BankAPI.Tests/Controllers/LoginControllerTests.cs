
using System.IdentityModel.Tokens.Jwt;
using BankAPI.Controllers;
using BankAPI.Models;
using BankAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace BankAPI.Tests.Controllers;
public class LoginControllerTests
{
	private readonly Mock<IUserService> _userService;
	private readonly LoginController _loginController;

    public LoginControllerTests()
    {
        _userService = new Mock<IUserService>();
        _loginController = new LoginController(_userService.Object);
    }

    [Fact]
    public void Authenticate_ValidCredentials_ReturnsOkObjectResult()
    {
        var user = new User { Username = "bankUser", Password = "bankPassword" };
        _userService.Setup(x => x.Authenticate(user.Username, user.Password)).Returns(true);

        var result = _loginController.Authenticate(user);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Authenticate_InvalidCredentials_ReturnsUnauthorizedResult()
    {
        var user = new User { Username = "invalidUser", Password = "invalidPassword" };
        _userService.Setup(x => x.Authenticate(user.Username, user.Password)).Returns(false);

        var result = _loginController.Authenticate(user);

        Assert.IsType<UnauthorizedResult>(result);
    }
}


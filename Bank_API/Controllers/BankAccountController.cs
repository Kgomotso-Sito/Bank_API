using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;

namespace BankAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly ILogger<BankAccountController> _logger;
    private readonly IAccountHolderService _accountHolderService;
    private readonly IBankAccountService _bankAccountService;

    public BankAccountController(ILogger<BankAccountController> logger,
        IAccountHolderService accountHolderService,
        IBankAccountService bankAccountService)
    {
        _logger = logger;
        _accountHolderService = accountHolderService;
        _bankAccountService = bankAccountService;
    }
        
    [HttpGet("GetBankAccountByAccountNumber")]
    public async Task<IActionResult> GetBankAccountByAccountNumber([FromQuery] string AccountNumber)
    {
        try
        {
            if (IsValidAccountNumber(AccountNumber))
            {
                return BadRequest("Account number is invalid");
            }

            var bankAccount = await _bankAccountService.GetBankAccountByAccountNumber(AccountNumber);

            if (bankAccount == null)
            {
                return BadRequest("No Account found");
            }

            return Ok(bankAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("GetBankAccountsByIdNumber")]
    public async Task<IActionResult> GetBankAccountsByIdNumber([FromQuery] string IdNumber)
    {
        if (IsValidSouthAfricanIdNumber(IdNumber))
        {
            return BadRequest("ID number is invalid");
        }

        try
        {
            var accountHolder = await _accountHolderService.GetAccountHolderByIdNumber(IdNumber);

            if (accountHolder == null)
            {
                return BadRequest("Accounts not found");
            }

            var bankAccounts = await _bankAccountService.GetBankAccountsByAccountHolderId(accountHolder.Id);

            if (bankAccounts == null || !bankAccounts.Any())
            {
                return BadRequest("Accounts not found");
            }

            return Ok(bankAccounts);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public static bool IsValidSouthAfricanIdNumber(string IdNumber)
    {
        return IdNumber.Length != 13 || !IdNumber.All(char.IsDigit);
    }

    public static bool IsValidAccountNumber(string AccountNumber)
    {
        return !AccountNumber.All(char.IsDigit);
    }

}


using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models;

public class WithdrawalRequest
{ 
    [Required]
    public string AccountNumber { get; set; }

    [Required]
    public decimal Amount { get; set; }
}
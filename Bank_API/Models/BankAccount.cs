using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BankAPI.Models;

[Table("Bank_Accounts")]
public class BankAccount
{
    public int Id { get; set; }
    public string AccountNumber { get; set; }
    public AccountType AccountType { get; set; }
    public string Name { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public decimal AvailableBalance { get; set; }

    [JsonIgnore]
    [ForeignKey("AccountHolderId")]
    public AccountHolder AccountHolder { get; set; }
}

public enum AccountType
{
    Cheque,
    Savings,
    FixedDeposit
}

public enum AccountStatus
{
    Active,
    Inactive
}


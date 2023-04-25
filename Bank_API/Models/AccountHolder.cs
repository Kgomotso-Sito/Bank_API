using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models;

[Table("Account_Holders")]
public class AccountHolder
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string IdNumber { get; set; }
    public string ResidentialAddress { get; set; }
    public string MobileNumber { get; set; }
    public string EmailAddress { get; set; }

    public ICollection<BankAccount> BankAccounts { get; set; }
}

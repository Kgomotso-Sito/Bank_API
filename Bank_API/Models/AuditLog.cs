using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models;

[Table("AuditLogs")]
public class AuditLog
{
    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }

    [ForeignKey("AccountHolderId")]
    public AccountHolder AccountHolder { get; set; }
    
    [ForeignKey("BankAccountId")]
    public BankAccount BankAccount { get; set; }
}

public enum Action
{
    Withdraw
}
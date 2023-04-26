using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models;

[Table("Audit_Logs")]
public class AuditLog
{
    public int Id { get; set; }
    public Action Action { get; set; }
    public DateTime Timestamp { get; set; }
    
    [ForeignKey("BankAccountId")]
    public BankAccount BankAccount { get; set; }
}

public enum Action
{
    Withdraw
}
using System.Transactions;
using BankAPI.Models;

namespace BankAPI.Repositories;

public interface IAuditLogRepository
{
    void SaveAuditLog(AuditLog auditLog);
}



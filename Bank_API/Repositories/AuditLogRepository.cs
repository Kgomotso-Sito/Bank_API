using System.Transactions;
using Microsoft.EntityFrameworkCore;
using BankAPI.Models;

namespace BankAPI.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly BankAccountDbContext _context;

    public AuditLogRepository(BankAccountDbContext context)
    {
        _context = context;
    }

    public void SaveAuditLog(AuditLog auditLog)
    {
        _context.AuditLogs.Add(auditLog);
        _context.SaveChanges();
    }
}



using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_API.Migrations
{
    /// <inheritdoc />
    public partial class TestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert test data for AccountHolders table
            migrationBuilder.InsertData(
                table: "Account_Holders",
                columns: new[] { "Id", "FirstName", "LastName", "DateOfBirth", "IdNumber", "ResidentialAddress", "MobileNumber", "EmailAddress" },
                values: new object[,]
                {
                    { 1, "John", "Doe", new DateTime(1980, 1, 1), "1234567891011", "123 Main St", "555-1234", "john.doe@example.com" },
                    { 2, "Jane", "Doe", new DateTime(1985, 1, 1), "2345678901111", "456 Oak St", "555-5678", "jane.doe@example.com" }
                });

            // Insert test data for BankAccounts table
            migrationBuilder.InsertData(
                table: "Bank_Accounts",
                columns: new[] { "Id", "AccountNumber", "AccountType", "Name", "AccountStatus", "AvailableBalance", "AccountHolderId" },
                values: new object[,]
                {
                    { 1, "1234567890", 0, "Savings Account", 0, 1000.00m, 1 },
                    { 2, "2345678901", 1, "Checking Account", 0, 500.00m, 1 },
                    { 3, "3456789012", 0, "Savings Account", 1, 2500.00m, 2 },
                    { 4, "4567890123", 1, "Checking Account", 1, 750.00m, 2 },
                    { 5, "9657780123", 2, "Fixed Deposit Account", 1, 750.00m, 2 }

                });

            // Insert test data for AuditLogs table
            migrationBuilder.InsertData(
                table: "Audit_Logs",
                columns: new[] { "Id", "Action", "Timestamp", "BankAccountId" },
                values: new object[,]
                {
                    { 1, 0, DateTime.Now, 1 },
                    { 2, 0, DateTime.Now, 2 },
                    { 3, 0, DateTime.Now, 3 },
                    { 4, 0, DateTime.Now, 4 }
                });
        }
        /// <inheritdoc />

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the test data from the tables
            migrationBuilder.DeleteData(
                table: "Bank_Accounts",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4 });

            migrationBuilder.DeleteData(
                table: "Account_Holders",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "Audit_Logs",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4 });
        }
    }
}

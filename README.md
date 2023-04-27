# Bank_API

## Micro-service application that exposes a secure REST API for consumers to:
  1. Retrieve a list of bank accounts for a given account holder. Each account must provide an account number, account type (Cheque, Savings, Fixed Deposit), name, status and available balance.
  2. Retrieve a single bank account for a given account number Create a new withdrawal for a given bank account.
  3. Create a new withdrawal for a given bank account.

## Technologies
  * C#. Net
  * MsSQL
  * Swagger
  * JWT

## Database
   * docker pull mcr.microsoft.com/mssql/server
   * docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<StrongPassword123>" -p 1433:1433 --name BankAccountDb -d mcr.microsoft.com/mssql/server
   
## Additionl infomation
  * Swagger runs on https://localhost:7174/swagger/index.html
  * login to authenticate use { "username": "bankUser", "password": "bankPassword"}
  * User the token to unlock API calls
  * Test Data, Id Number: 1234567891011 and Account Number: 1234567890

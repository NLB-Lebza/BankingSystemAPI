using BankingSystemAPI.Models;
using BankingSystemAPI.DTOs;

namespace BankingSystemAPI.Services;
public interface IAccountService
{
  Task<BankAccount> CreateAsync(CreateAccountDto dto);
Task<BankAccount?> GetBankAccountNumberAsync(string accountNumber);
Task<BankAccount?> DepositAsync(DepositDto dto);
Task<BankAccount?> WithdrawAsync(WithdrawDto dto);

Task TransferAsync (TransferDto dto);//inside here the is transfer method that will be used to transfer money from one account to another

Task<IEnumerable<Transaction>> GetTransactionsByAccountNumberAsync(string accountNumber);

}
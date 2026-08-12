using BankingSystemAPI.Models;
namespace BankingSystemAPI.Repositories;

public interface IAccountRepo
{
    Task<BankAccount> AddAsync(BankAccount account);
    Task<BankAccount?> GetByAccountNumberAsync(string accountNumber);


    Task<IEnumerable<Transaction>> GetTransactionsAsync(
        string accountNumber);
    Task SaveChangesAsync();
}
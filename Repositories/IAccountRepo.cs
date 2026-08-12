using BankingSystemAPI.Models;
namespace BankingSystemAPI.Repositories
{
    public interface IAccountRepo
    {
       Task<BankAccount?> GetByAccountNumberAsync(string accountNumber);
        Task<BankAccount>AddAsync(BankAccount account);
        Task<IEnumerable<Transaction>>GetTransactionAsync(string accountNumber);
        Task saveChangesAsync();
    }
}
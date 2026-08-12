using BankingSystemAPI.Models;

namespace BankingSystemAPI.Repositories;

public interface ICustomerRepo
{
    // The service uses this interface instead of talking directly to EF Core.
    Task<Customer> AddAsync(Customer customer);

    Task<Customer?> GetByIdAsync(int id);

    Task<Customer?> GetByEmailAsync(string email);
  
    Task<IEnumerable<Customer>> GetAllAsync();
}
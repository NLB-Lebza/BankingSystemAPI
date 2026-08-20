using BankingSystemAPI.DTOs;
using BankingSystemAPI.Models;

namespace BankingSystemAPI.Services;

public interface ICustomerService
{
    Task<Customer> CreateAsync(CreateCustomerDto dto);
    Task<Customer?> GetByIdAsync(int id);
    Task<IEnumerable<Customer>> GetAllAsync();
}


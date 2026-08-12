using BankingSystemAPI.Data;
using BankingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPI.Repositories;

//Repo => DBcontext=> SQL Server

public class CustomerRepo : ICustomerRepo
{
    private readonly BankingDbContext _context;

    public CustomerRepo(BankingDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .Include(c=> c.BankAccounts)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers
        .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Include(c => c.BankAccounts)
            .ToListAsync();
    }
}

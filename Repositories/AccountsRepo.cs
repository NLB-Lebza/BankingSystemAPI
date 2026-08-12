using BankingSystemAPI.Models;
using BankingSystemAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPI.Repositories;
// This repository performs account and transaction database operations

public class AccountsRepo : IAccountRepo
{
    private readonly BankingDbContext _context;

    public AccountsRepo(BankingDbContext context)
    { _context = context; }

   
    public async Task<BankAccount?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _context.BankAccounts
            .Include(a => a.Customer)
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }

    public async Task<BankAccount> AddAsync(BankAccount account)
    {
        _context.BankAccounts.Add(account);
        await _context.SaveChangesAsync();
        return account;
    }


    public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string accountNumber)
    {
        return await _context.Transactions
            .Where(t => t.BankAccount!.AccountNumber == accountNumber)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
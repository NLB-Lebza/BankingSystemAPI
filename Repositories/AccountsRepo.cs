using BankingSystemAPI.Models;
using BankingSystemAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPI.Repositories;
// This repository performs account and transaction database operations

public class AccountsRepo : IAccountRepo
{
    private readonly BankingSystemAPI _context;

    public AccountsRepo(BankingSystemAPI context)
    {
        this._context = context;
    }

    public async Task<BankAccount?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _context.BankAccount
            .Include(a => a.Customer)
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }
    public async Task<IEnumerable<Transactions>> GetTransactionsAsync(string accountNumber)
    {
        return await _context.Transactions
            .Where(t => t.BankAccount.AccountNumber == accountNumber)
            .OrderbyDescending(t => t.CreatedAt)
            .ToListAsync();

    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
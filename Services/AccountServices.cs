using BankingSystemAPI.Models;
using BankingSystemAPI.Repositories;
using BankingSystemAPI.DTOs;
using BankingSystemAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPI.Services;

    public class AccountServices : IAccountService
{
        public readonly IAccountRepo _accountRepo;
        private readonly BankingDbContext _context;
        private readonly ICustomerRepo _customerRepo;

        public AccountServices(IAccountRepo accountRepo , BankingDbContext context, ICustomerRepo customerRepo)
        {
            _accountRepo = accountRepo;
            _context = context;
            _customerRepo = customerRepo;
        }
public async Task<BankAccount> CreateAsync(CreateAccountDto dto)
    {
        //Validating the input data
        var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
        if(customer ==null)
        
            throw new ArgumentException("Customer not found");

        var account = new BankAccount
        {
            CustomerId = dto.CustomerId,
            AccountNumber = await GenerateUniqueAccountNumberAsync(),
                 AccountType = dto.AccountType.Trim(),
            Balance = dto.InitialDeposit,
       
        };   

        //saving the account to the database
        await _accountRepo.AddAsync(account);
//recordinmg the opening transaction if the initial deposit is greater than 0
        if(dto.InitialDeposit> 0)
        {
            _context.Transactions.Add(new Transaction
            {
                
            
                BankAccountId = account.Id,
                Type= TransactionType.Deposit,
                Amount = dto.InitialDeposit,
                Description = "Initial deposit",
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
        return account;
        }

        public Task<BankAccount?> GetByAccountNumberAsync(string accountNumber)
        {
            return _accountRepo.GetByAccountNumberAsync(accountNumber);
        }

        public async Task<BankAccount> DepositAsync(DepositDto dto)
        {
        if (string.IsNullOrEmpty(dto.AccountNumber))
        {
            throw new ArgumentException("Account number is required");
        }

        if (dto.Amount <= 0)
        {
            throw new ArgumentException("Deposit amount must be greater than zero");
        }
        if(dto.Amount > 10000)
        {
            throw new ArgumentException("Deposit amount must be less than or equal to 10,000");
        }

        var account = await RequiredAccountAsync(dto.AccountNumber);
        account.Balance += dto.Amount;

        _context.Transactions.Add(new Transaction{
            BankAccountId = account.Id,
            Amount = dto.Amount,
            Type = TransactionType.Deposit,
            Description = "Cash Deposit",
            CreatedAt = DateTime.UtcNow
        });
        await _accountRepo.SaveChangesAsync();
        return account;
        }
        
    public async Task<BankAccount>WithdrawAsync(WithdrawDto dto)
    {
        if (string.IsNullOrEmpty(dto.AccountNumber))
        {
            throw new ArgumentException("Account number is required");
        }

        if (dto.Amount <= 0)
        {
            throw new ArgumentException("Withdrawal amount must be greater than zero");
        }

        var account = await RequiredAccountAsync(dto.AccountNumber);

        if (account.Balance < dto.Amount)
        {
            throw new InvalidOperationException("Insufficient funds for withdrawal");
        }

        account.Balance -= dto.Amount;

        _context.Transactions.Add(new Transaction
        {
            BankAccountId = account.Id,
            Amount = dto.Amount,
            Type = TransactionType .Withdrawal,
            Description = "Cash Withdrawal",
            CreatedAt = DateTime.UtcNow
        });

        await _accountRepo.SaveChangesAsync();
        return account;

    }

    public async Task TransaferAsync(TransferDto dto)
    {
        //check for if its empty
        if (string.IsNullOrEmpty(dto.FromAccountNumber))
        {
            throw new ArgumentException("From account number is required");
        }
        if (string.IsNullOrEmpty(dto.ToAccountNumber))
        {
            throw new ArgumentException("To account number is required");
        }
        //check if transafer has amount >0
        if (dto.Amount <= 0)
        {
            throw new ArgumentException("Transfer amount must be greater than zero");
        }
        //check if the amount doesnt pass the limit of 10,000
        if (dto.Amount > 10000)
        {
            throw new ArgumentException("Transfer amount must be less than or equal to 10,000");
        }
        //check if the sender and receiver are the same
        if (dto.FromAccountNumber == dto.ToAccountNumber)
        {
            throw new ArgumentException("Cannot transfer to the same account");
        }

        //start a data base transaction basically tgis make sure all changes succeed or fail together
        await using var databaseTransaction = await _context.Database.BeginTransactionAsync();
        try
        {
            //get the sender account from DB
            var senderAccount = await RequiredAccountAsync(dto.FromAccountNumber);

            var receiverAccount = await RequiredAccountAsync(dto.ToAccountNumber);


// this is for checking if the sender has enough balance to transfer
            if(senderAccount.Balance< dto.Amount)
            {
                throw new InvalidOperationException("Insufficient funds for transfer");
            }

            //deduct the amount from the sender account
            senderAccount.Balance -= dto.Amount;

            //add the amount to the receiver account
            receiverAccount.Balance += dto.Amount;

            //creates a unique reference number for the transaction
            var referenceNumber = $"TRF-{Guid.NewGuid():N}".ToUpper();

            //create a transaction record for the sender
            _context.Transactions.AddRange(new Transaction
            {
                //this connects the transaction to the sender account
                BankAccountId= senderAccount.Id,
                Amount = dto.Amount,
            //marks the money as it leaves the sender account
                Type = TransactionType.TransferOut,
                Description = $"Transfer to {receiverAccount.AccountNumber}",
                Referance = referenceNumber,
                CreatedAt = DateTime.UtcNow
            });
   //this makes all the data changes to the database and saves them
            await databaseTransaction.CommitAsync();

        }
        catch
        {
            await databaseTransaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string accountNumber)
    {
        await RequiredAccountAsync(accountNumber);
        return await _accountRepo.GetTransactionsAsync(accountNumber);
    }

    private async Task<BankAccount> RequiredAccountAsync(string accountNumber)
    {
        var account = await _accountRepo.GetByAccountNumberAsync(accountNumber);
        if(account ==null)
        throw new ArgumentException($"Account with number {accountNumber} not found");

    return account;
    
    }
    private async Task<string> GenerateUniqueAccountNumberAsync()
    {
        string accountNumber;
        do
        {
            accountNumber = Random.Shared.NextInt64(1000000000, 9999999999).ToString();
        } while (await _accountRepo.GetByAccountNumberAsync(accountNumber) != null);

        return accountNumber;
    }

    public Task<BankAccount?> GetBankAccountNumberAsync(string accountNumber)
    {
        throw new NotImplementedException();
    }

    public Task TransferAsync(TransferDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Transaction>> GetTransactionsByAccountNumberAsync(string accountNumber)
    {
        throw new NotImplementedException();
    }
}
    
    

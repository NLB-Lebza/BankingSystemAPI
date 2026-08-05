using BankingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingDbContext
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(BankingDbContextOptions options) : base(options)
        { 
        }

        //prop becomes database table
        public DbSet<Customer> Customer => Set<Customer>();
        public DbSet<BankAccount> BankAccount => Set<BankAccount>();
        public  DbSet<Transaction> Transaction => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationships and constraints

            //Enail should be unique for each customer
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .isUnique();

            //account number should be unique for each bank account
            modelBuilder.Entity<BankAccount>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();

            //Configure money column in the server
            modelBuilder.Entity<BankAccount>()
                .Property(a => a.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            //Customer can have many bank accounts, but each bank account belongs to one customer
            modelBuilder.Entity<BankAccount>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.BankAccounts)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restricted);


            //Bank account can have many transactions, but each transaction belongs to one bank account
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.BankAccount)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.BankAccountId)
                .OnDelete(DeleteBehavior.Restricted);


        }
    }
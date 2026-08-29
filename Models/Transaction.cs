namespace BankingSystemAPI.Models;

public class Transaction
{
    public int Id { get; set; }

    public TransactionType Type { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }= string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //FK to the BankAccount entity
    public int BankAccountId { get; set; }
    
    //Navigation property to the BankAccount entity
    public BankAccount? BankAccount { get; set; }

    //links to the other bank account in case of transfer(INCOMING AND OUTGOING)
    public string? Referance { get; set; }
}
namespace BankingSystem.Models
{
    public class BankAccount
    {
        public int Id { get; set; }

        public TransactionType TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }=string.Empty;

        public DateTime CreatedAt { get; set; }

        //FK to the BankAccount entity
        public int BankAccountId { get; set; }
        
        //Navigation property to the BankAccount entity
        public BankAccount? BankAccount { get; set; }

        //links to the other bank account in case of transfer(INCOMING AND OUTGOING)
        public string? Referance { get; set; }
    }
}
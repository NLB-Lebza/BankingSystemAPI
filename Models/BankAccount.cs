namespace BankingSystemAPI.Models;

    public class BankAccount
    {
        public int Id { get; set; }

    //Foreign key to the Customer entity
    public int CustomerId { get; set; }
    //the navigation property to the Customer entity
    public Customer? Customer { get; set; }

    public string AccountNumber { get; set; }

        public string AccountType { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreatedAt { get; set; }




    //Navigation property to the Transaction entity and one account can have many transactions
    public ICollection<Transaction> Transactions { get; set; }
        = new List<Transaction>();
}

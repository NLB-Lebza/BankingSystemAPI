namespace BankingSystem.Models
{
    public class Customer
    {

        public int Id { get; set; }
        public int Name { get; set; }
        public int Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    }

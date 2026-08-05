namespace BankingSystem.DTOs
{
    public class CreateCustomerDto
    {
        public string CustomerId { get; set; }

        public string AccountType { get; set; }

        public decimal InitialDeposit { get; set; }
    }
}
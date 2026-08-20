namespace BankingSystemAPI.DTOs;

public class CreateAccountDto
{ 
    public string CustomerId { get; set; }

   public string AccountType { get; set; }

    public decimal InitialDeposit { get; set; }
}
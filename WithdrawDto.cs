namespace BankingSytem.DTOs
{
    public class WithdrawDto
    {
      public string FromAccountNumber { get; set; }

        public string ToAccountNumber { get; set; }

        public decimal Amount { get; set; }
    }
}
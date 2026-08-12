namespace BankingSystemAPI.Models;

public enum TansactionType
{
    //this enum is used to define the type of transaction that can be performed on a bank account. The values are:
   Deposit = 1, 
   Withdraw = 2,
   TransferOut = 3,
   TransferIn= 4

}
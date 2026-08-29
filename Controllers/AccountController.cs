using BankingSystemAPI.DTOs;
using BankingSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BankingSystemAPI.Controller;

[ApiController]
[Route("api/[controller]")]

public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> Create (CreateAccountDto dto)
    {
        try
        {
            var account = await _accountService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByAccountNumber),
            new{accountNumber = account.AccountNumber}, account);
        }catch(KeyNotFoundException ex)
        {
            return NotFound(new{message = ex.Message});
        }
    }
    [HttpGet("{accountNumber}")]
    public async Task<IActionResult> GetByAccountNumber(string accountNumber)
    {
        var account = await _accountService.GetBankAccountNumberAsync(accountNumber);
        return account == null
        ? NotFound(new{message ="Bank account was not found"})
        :Ok(account);
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(DepositDto dto)
    {
        try
        {
            var account = await _accountService.DepositAsync(dto);
            return Ok(account);

        }
        catch (ArgumentException ex)
{
    return BadRequest(new
    {
        message = ex.Message
    });


    }
    catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                meessage =ex.Message
            });
        }
    }
    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(WithdrawDto dto)
    {
        try
        {
            return Ok(await _accountService.WithdrawAsync(dto));
        }catch(KeyNotFoundException ex)
        {
            return NotFound(new {message = ex.Message});
        }
        catch(InvalidOperationException ex)
        {
          return BadRequest(new {message =ex.Message});
        }
    }
    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer (TransferDto dto)
    {
        try
        {
            await _accountService.TransferAsync(dto);
            return Ok(new{message="Transfer complete successfuly"});
        }catch(KeyNotFoundException ex)
        {
            return NotFound(new{message = ex.Message});
        }
        catch(InvalidOperationException ex)
        {
            return BadRequest(new{message = ex.Message});
        }
        
    }
    [HttpGet("{accountNumber}/tramsactions")]
    public async Task<IActionResult> GetTransaction(string accountNumber)
    {
        try
        {
            return Ok(await _accountService.GetTransactionsByAccountNumberAsync(accountNumber));
        }catch(KeyNotFoundException ex)
        {
            return NotFound(new{message = ex.Message});
        }
    }
}


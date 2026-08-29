using BankingSystemAPI.DTOs;
using BankingSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystemAPI.Controller;

[ApiController]
[Route("api/[controller]")]

public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;


public CustomerController(ICustomerService customerService)
    {
        _customerService= customerService;
    }

[HttpPost]
public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        try
        {
            var customer = await _customerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById),new {id=customer.Id},customer);
        }
        catch(InvalidOperationException ex )
        {
            return Conflict(new{ message = ex.Message});
        }
    }
    [HttpGet]
    public async Task<IActionResult>GetAll()
    => Ok(await _customerService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        return customer ==null
        ? NotFound(new{message="Customer was not found."})
        :Ok(customer);
    }
}
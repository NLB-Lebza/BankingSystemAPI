using BankingSystemAPI.DTOs;
using BankingSystemAPI.Models;
using BankingSystemAPI.Repositories;

namespace BankingSystemAPI.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepo _customerRepository;//field for the customer repository

    public CustomerService(ICustomerRepo customerRepository)//Dependency injection of the customer repository, this so that customerser acces the repo
    {
        _customerRepository = customerRepository;
    }

    //method to recive the customer information from the controller and pass it to the repository to create a new customer/combined with validations
    public async Task<Customer> CreateAsync(CreateCustomerDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("Name is required");
            //throw new ArgumentException("Name is required", nameof(dto.Name));
        }
        //length of the name
        if (dto.Name.Length > 100)
        {
            throw new ArgumentException("Name can not be longer than 50 characters");
        }

        //check if the surname contains white spaces
        if (string.IsNullOrWhiteSpace(dto.Surname))
        {
            throw new ArgumentException("surname is required");
        }

        //checking the length of the surname
        if (dto.Surname.Length > 100)
        {
                        throw new ArgumentException("Surname can not be longer than 50 characters");
        }

        //check if the email is valid
        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new ArgumentException("Email is required");
        }

        if (dto.Email.Length > 100)
        {
            throw new ArgumentException("Email can not be longer than 100 characters");
        }

        //check if it contains the @ symbol
        if(!dto.Email.Contains("@"))
        {
            throw new ArgumentException("Email is not valid");
        }
        //check if the phone number is valid
        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            throw new ArgumentException("Phone number is required");
        }

        //check if the phone number is valid
        if (dto.PhoneNumber.Length > 15)
        {
            throw new ArgumentException("Phone number can not be longer than 15 characters");
        }

        //check if the phone number contains only digits
        if(!dto.PhoneNumber.All(char.IsDigit))
        {
            throw new ArgumentException("Phone number can only contain digits");
        }

        //checking if the email already exists in the database
        var existing = await _customerRepository 
            .GetByEmailAsync(dto.Email);
        if (existing != null)
        {
            throw new ArgumentException("Email already exists");
        }

        //if it doesnt exist create a new customer using DTO Data

        var customer = new Customer
        {
            Name = dto.Name.Trim(),
            Surname = dto.Surname.Trim(),
            Email = dto.Email.Trim().ToLower(),
        };
        return await _customerRepository.AddAsync(customer);
    }
    //gets one customer by id from the repository and returns it to the controller
    public Task<Customer> GetByIdAsync(int id)
    {
        return _customerRepository.GetByIdAsync(id);
    }

    //gets all customers from the repository and returns it to the controller
    public Task<IEnumerable<Customer>> GetAllAsync()
    {
        return _customerRepository.GetAllAsync();
    }
}
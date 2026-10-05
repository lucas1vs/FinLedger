using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLedgerDomain.Entities;

public class Customer
{
    public int CustomerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string CPF { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public decimal MonthlyIncome { get; private set; }

    //Relação Customer 1:N Account
    private readonly List<Account> _accounts = new List<Account>();
    public IReadOnlyCollection<Account> Accounts => _accounts;



    private Customer() { }

    public Customer(string name, string cpf, DateTime dateOfBirth, string email, string phone, decimal monthlyIncome)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome não pode ser vazio.", nameof(name));

        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF não pode ser vazio.", nameof(cpf));

        if (dateOfBirth > DateTime.UtcNow)
            throw new ArgumentOutOfRangeException(nameof(dateOfBirth), "Data de nascimento não pode ser no futuro.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email não pode ser vazio.", nameof(email));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone não pode ser vazio.", nameof(phone));

        if (monthlyIncome < 0)
            throw new ArgumentOutOfRangeException(nameof(monthlyIncome), "Renda mensal não pode ser negativa.");

        Name = name;
        CPF = cpf;
        DateOfBirth = dateOfBirth;
        Email = email;
        Phone = phone;
        MonthlyIncome = monthlyIncome;
    }
}

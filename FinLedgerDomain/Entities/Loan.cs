using FinLedgerDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FinLedgerDomain.Entities;

public class Loan
{
    public int LoanId { get; private set; }
    public decimal RequestedAmount { get; private set; }
    public decimal InterestRate { get; private set; }
    public int NumberOfInstallments { get; private set; }
    public AmortizationSystem AmortizationSystem { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? ContractedAt { get; private set; }
    public LoanStatus Status { get; private set; }

    //Relação Account 1:N Loan
    public Account Account { get; private set; } = null!; 
    public int AccountId { get; private set; }

    //RelaçãoLoan 1:N Installment
    private readonly List<Installment> _installments = new List<Installment>();
    public IReadOnlyCollection<Installment> Installments => _installments;

    private Loan() { }

    public Loan(decimal requestedAmount, decimal interestRate, int numberOfInstallments,
        AmortizationSystem amortizationSystem,int accountId)
    {
        if (requestedAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(requestedAmount), "Requested amount nao pode ser negativa.");
        }

        if (interestRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(interestRate), "Taxa de juros nao pode ser menor que zero.");
        }

        if (numberOfInstallments <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfInstallments), "O número de parcelas nao pode ser negativo.");
        }

        if (accountId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(accountId), "O ID da conta tem que ser maior que zero.");
        }

        RequestedAmount = requestedAmount;
        InterestRate = interestRate;
        NumberOfInstallments = numberOfInstallments;
        AmortizationSystem = amortizationSystem;
        AccountId = accountId;
        Status = LoanStatus.Requested;
        RequestedAt = DateTime.UtcNow; 

    }

    public void Approve()
    {
       
        if (Status != LoanStatus.Requested)
           throw new InvalidOperationException("Só é possível aprovar um empréstimo com status Requested.");

        Status = LoanStatus.Approved;
        ContractedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        if (Status != LoanStatus.Requested)
            throw new InvalidOperationException("Só é possível rejeitar um empréstimo com status Requested.");

        Status = LoanStatus.Rejected;
    }

}

using FinLedgerDomain.Enums;

namespace FinLedgerDomain.Entities;

public class Installment
{
    public int InstallmentId { get; private set; }
    public int Number { get; private set; }
    public decimal Amount { get; private set; }
    public decimal Interest { get; private set; }
    public decimal Principal { get; private set; }
    public decimal RemainingBalance { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public InstallmentStatus Status { get; private set; }

    //RelaçãoLoan 1:N Installment
    public Loan Loan { get; private set; } = null!;
    public int LoanId { get; private set; }

    private Installment() { }

    public Installment(int number, decimal amount, decimal interest, decimal principal, decimal remainingBalance, 
        DateTime dueDate, int loanId)
    {
        if (number <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "O número da parcela deve ser maior que zero.");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount deve ser maior que zero.");
        }

        if (interest < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(interest), "Interest nao pode ser negativo.");
        }

        if (principal <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(principal), "Principal deve ser maior que zero.");
        }

        if (remainingBalance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(remainingBalance), "RemainingBalance nao pode ser negativo.");
        }

        if (loanId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(loanId), "O LoanId  nao pode ser negativo.");
        }

        Number = number;
        Amount = amount;
        Interest = interest;
        Principal = principal;
        RemainingBalance = remainingBalance;
        LoanId = loanId;
        Status = InstallmentStatus.Pending;
        PaidAt = null;
        DueDate = dueDate;
        


    }

    public void MarkAsPaid()
    {
        if (Status == InstallmentStatus.Pending)
        {
           
            PaidAt = DateTime.UtcNow;
            Status = InstallmentStatus.Paid;

        }

        if (Status == InstallmentStatus.Paid)
        {
            
            throw new InvalidOperationException("A parcela já foi paga.");
        }
    }



}

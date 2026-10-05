using FinLedgerDomain.Enums;

namespace FinLedgerDomain.Entities;

public class Account
{

    public int AccountId { get; private set; }
    public string Number { get; private set; } = string.Empty;
    public DateTime OpenedAt { get; private set; }
    public AccountStatus Status { get; private set; }
    public decimal OverdraftLimit { get; private set; }

    //Relação Customer 1:N Account
    public Customer Customer { get; private set; } = null!;
    public int CustomerId { get; private set; }

    //Relação Account 1:N Loan
    private readonly List<Loan> _loans = new List<Loan>();
    public IReadOnlyCollection<Loan> Loans => _loans;

    //Relação Account 1:N Transaction

    private readonly List<Transaction> _transactions = new List<Transaction>();
    public IReadOnlyCollection<Transaction> Transactions => _transactions;



    private Account() { }

    public Account(string number, int customerId, decimal overdraftLimit)
    {
        if (customerId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(customerId), "CustomerId deve ser maior que zero.");
        }

        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("Número da conta não pode ser nulo ou vazio.", nameof(number));
        }
        if (overdraftLimit < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(overdraftLimit), "O limite de cheque especial não pode ser negativo.");
        }

        Number = number;
        CustomerId = customerId;
        Status = AccountStatus.Active;
        OpenedAt = DateTime.UtcNow;
        OverdraftLimit = overdraftLimit;

        

    }


    public void Block()
    {
        if (Status == AccountStatus.Closed)
            throw new InvalidOperationException("Não é possível bloquear uma conta fechada.");

        if (Status == AccountStatus.Blocked)
            throw new InvalidOperationException("A conta já está bloqueada.");

        if (HasOpenLoan())
            throw new InvalidOperationException("Não é possível bloquear uma conta com empréstimos em aberto.");

        Status = AccountStatus.Blocked;
    }

    public void Close()
    {
        if (Status == AccountStatus.Closed)
            throw new InvalidOperationException("A conta já está fechada.");

        if (HasOpenLoan())
            throw new InvalidOperationException("Não é possível fechar uma conta com empréstimos em aberto.");

        Status = AccountStatus.Closed;
    }

    public void Activate()
    {
        if (Status == AccountStatus.Closed)
            throw new InvalidOperationException("Não é possível ativar uma conta fechada.");

        if (Status == AccountStatus.Active)
            throw new InvalidOperationException("A conta já está ativa.");

        Status = AccountStatus.Active;
    }

    /// <summary>
    /// Calcula o saldo somando as transações da conta (Credit soma, Debit subtrai).
    /// Só é confiável se TODAS as transações da conta estiverem carregadas em Transactions.
    /// </summary>
    public decimal GetBalance()
    {
        return Transactions.Sum(t => t.Direction == TransactionDirection.Credit ? t.Amount : -t.Amount);
    }
    private bool HasOpenLoan()
    {
        return Loans.Any(loan => loan.Status != LoanStatus.PaidOff
                              && loan.Status != LoanStatus.Rejected);
    }


    



}







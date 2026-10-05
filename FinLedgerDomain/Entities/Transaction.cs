using FinLedgerDomain.Entities;
using FinLedgerDomain.Enums;

namespace FinLedgerDomain.Entities;

public class Transaction
{
    public int TransactionId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public TransactionDirection Direction { get; private set; }
    public DateTime OccurredAt { get; private set; }
    public string Description { get; private set; } = string.Empty;

    // Account 1:N Transaction
    public int AccountId { get; private set; }
    public Account Account { get; private set; } = null!;

    // Installment 1:0..1 Transaction
    public int? InstallmentId { get; private set; }
    public Installment? Installment { get; private set; }

    // Usado pelo EF Core para materializar
    private Transaction() { }

    private Transaction(
        decimal amount,
        int accountId,
        TransactionType type,
        TransactionDirection direction,
        DateTime occurredAt,
        string? description,
        int? installmentId)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "O valor da transação deve ser maior que zero.");

        if (accountId <= 0)
            throw new ArgumentOutOfRangeException(nameof(accountId), "AccountId deve ser maior que zero.");

        if (installmentId.HasValue && installmentId.Value <= 0)
            throw new ArgumentOutOfRangeException(nameof(installmentId), "InstallmentId, quando informado, deve ser maior que zero.");

        Amount = amount;
        AccountId = accountId;
        Type = type;
        Direction = direction;
        OccurredAt = occurredAt;
        Description = description ?? string.Empty;
        InstallmentId = installmentId;
    }

    public static Transaction CreateInstallmentPayment(
        decimal amount,
        int accountId,
        int installmentId,
        DateTime occurredAt,
        string? description = null)
    {
        return new Transaction(
            amount,
            accountId,
            TransactionType.InstallmentPayment,
            TransactionDirection.Debit,
            occurredAt,
            description,
            installmentId);
    }
}
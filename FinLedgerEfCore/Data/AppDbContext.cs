using FinLedgerDomain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
namespace FinLedgerEfCore.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Installment> Installments { get; set; }
    public DbSet<Loan> Loans { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(a => a.Number).HasMaxLength(20).IsRequired();
            entity.Property(a => a.OpenedAt).IsRequired();
            entity.Property(a => a.Status).IsRequired();
            entity.Property(a => a.OverdraftLimit).HasPrecision(18, 2).IsRequired();

            entity.HasOne(a => a.Customer)
                  .WithMany(c => c.Accounts)
                  .HasForeignKey(a => a.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(150).IsRequired();
            entity.Property(c => c.CPF).HasMaxLength(11).IsRequired();
            entity.HasCheckConstraint("CK_Customers_CPF_Valid", "[CPF] NOT LIKE '%[^0-9]%' AND LEN([CPF]) = 11");
            entity.Property(c => c.DateOfBirth).IsRequired();
            entity.Property(c => c.Email).HasMaxLength(150).IsRequired();
            entity.Property(c => c.Phone).HasMaxLength(20).IsRequired();
            entity.Property(c => c.MonthlyIncome).HasPrecision(18, 2).IsRequired();
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.Property(l => l.RequestedAmount).HasPrecision(18, 2).IsRequired();
            entity.Property(l => l.InterestRate).HasPrecision(5, 4).IsRequired();
            entity.Property(l => l.NumberOfInstallments).IsRequired();
            entity.Property(l => l.AmortizationSystem).IsRequired();
            entity.Property(l => l.Status).IsRequired();
            entity.Property(l => l.RequestedAt).IsRequired();
            entity.Property(l => l.ContractedAt).IsRequired(false);

            entity.HasOne(l => l.Account)
                  .WithMany(a => a.Loans)
                  .HasForeignKey(l => l.AccountId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Installment>(entity =>
        {
            entity.Property(i => i.Number).IsRequired();
            entity.Property(i => i.Amount).HasPrecision(18, 2).IsRequired();
            entity.Property(i => i.Interest).HasPrecision(18, 2).IsRequired();
            entity.Property(i => i.Principal).HasPrecision(18, 2).IsRequired();
            entity.Property(i => i.RemainingBalance).HasPrecision(18, 2).IsRequired();
            entity.Property(i => i.DueDate).IsRequired();
            entity.Property(i => i.PaidAt).IsRequired(false);
            entity.Property(i => i.Status).IsRequired();

            entity.HasOne(i => i.Loan)
                  .WithMany(l => l.Installments)
                  .HasForeignKey(i => i.LoanId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(t => t.Amount).HasPrecision(18, 2).IsRequired();
            entity.Property(t => t.Type).IsRequired();
            entity.Property(t => t.Direction).IsRequired();
            entity.Property(t => t.OccurredAt).IsRequired();
            entity.Property(t => t.Description).HasMaxLength(500).IsRequired(false);

            entity.HasOne(t => t.Account)
                  .WithMany(a => a.Transactions)
                  .HasForeignKey(t => t.AccountId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Installment)
                  .WithOne()
                  .HasForeignKey<Transaction>(t => t.InstallmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}













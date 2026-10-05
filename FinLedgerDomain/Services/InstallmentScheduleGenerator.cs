using FinLedgerDomain.Entities;
using FinLedgerDomain.Enums;
using System.ComponentModel;

namespace FinLedgerDomain.Calculators;

public static class InstallmentScheduleGenerator
{
    public static List<Installment> Generate(Loan loan)
    {
        return loan.AmortizationSystem switch
        {
            AmortizationSystem.Price => GeneratePrice(loan),
            AmortizationSystem.SAC => GenerateSac(loan),
            _ => throw new ArgumentOutOfRangeException(nameof(loan.AmortizationSystem))
        };
    }

    private static List<Installment> GenerateSac(Loan loan)
    {
        if (loan.ContractedAt is null)
        throw new InvalidOperationException("Não é possível gerar parcelas de um empréstimo sem contratação.");

        var taxa = loan.InterestRate;
        var n = loan.NumberOfInstallments;
        var valorEmprestado = loan.RequestedAmount;
        var database = loan.ContractedAt.Value.AddDays(30);

        var amortizacaoFixa = valorEmprestado / n;

        var lista=new List<Installment>();

        var saldoDevedor = valorEmprestado;

        for (int k = 1; k <= n; k++)
        {
            var juros = saldoDevedor * taxa;
            var parcela = amortizacaoFixa + juros;

            var novoSaldo = saldoDevedor - amortizacaoFixa;
            var vencimento = database.AddMonths(k - 1);

            var installment = new Installment(k, parcela, juros, amortizacaoFixa, novoSaldo, vencimento, loan.LoanId);
            lista.Add(installment);

            saldoDevedor = novoSaldo;

        }

        return lista;

    }

    private static List<Installment> GeneratePrice(Loan loan)
    {
        if (loan.ContractedAt is null)
            throw new InvalidOperationException("Não é possível gerar parcelas de um empréstimo sem contratação.");

        //X = Valor_Emprestado × i / (1 − (1 + i)^−n)
        var taxa = loan.InterestRate;
        var n = loan.NumberOfInstallments;
        var valorEmprestado = loan.RequestedAmount;
        var database = loan.ContractedAt.Value.AddDays(30);

        //passo 1: Calcular a parcela fixa (X)
        double fator = Math.Pow(1 + (double)taxa, -n);
        decimal resultado = valorEmprestado * taxa / (1 - (decimal)fator);


        var saldoDevedor = valorEmprestado;

        // Passo 2: iterar de 1 até n
        var lista = new List<Installment>();

        for (int k = 1; k <= n; k++)
        {
            var juros = saldoDevedor * taxa;
            var amortizacao = resultado - juros; //X = Resultado
            var novoSaldo = saldoDevedor - amortizacao;
            var vencimento = database.AddMonths(k - 1);

            var installment = new Installment(k, resultado, juros, amortizacao, novoSaldo, vencimento, loan.LoanId);
            lista.Add(installment);
        }

        return lista;
    }

}


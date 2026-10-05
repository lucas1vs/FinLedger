public static class OverdraftLimitCalculator
{
    private const decimal PercentageOfIncome = 0.20m;

    public static decimal Calculate(decimal monthlyIncome)
    {
        if (monthlyIncome <= 0)
            throw new ArgumentOutOfRangeException(nameof(monthlyIncome), "Monthly income must be greater than zero.");

        return monthlyIncome * PercentageOfIncome;
    }
}

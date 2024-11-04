namespace CashMapper
{
    /// <summary>
    /// DTO for an entry that represents an income line-item associated with an IncomeProfile.
    /// </summary>
    internal record IncomeEntry(
        long Id,
        string Name,
        long IncomeProfileId,
        decimal MonthlyValue
        )
    {
    }
}

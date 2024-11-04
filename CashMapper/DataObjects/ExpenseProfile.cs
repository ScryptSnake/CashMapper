namespace CashMapper
{
    /// <summary>
    /// DTO for an item that represents a known monthly expense. 
    /// </summary>
    internal record ExpenseProfile(
        long Id,
        string Description,
        decimal MonthlyValue,
        string Category,
        string? Note
        )
    {
    }
}

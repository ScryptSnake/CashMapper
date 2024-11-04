namespace CashMapper
{
    /// <summary>
    /// DTO for an item to be budgeted for. Example: 'Groceries'.
    /// </summary>
    internal record BudgetEntry(
        long Id,
        string Description,
        decimal MonthlyValue,
        string Category,
        string? Note
        )
    {
    }
}

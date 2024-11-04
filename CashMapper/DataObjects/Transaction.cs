namespace CashMapper
{
    /// <summary>
    /// DTO for a transaction entry to catalogue budget transactions.
    /// </summary>
    internal record Transaction(
        long Id,
        string Description,
        string Source,
        DateTime TransactionTime,
        decimal Value,
        string Category,
        string? Note
        )
    {
    }
}

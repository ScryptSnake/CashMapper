namespace CashMapper
{
    /// <summary>
    /// DTO for an entry that describes a balance recording of an account profile.
    /// </summary>
    internal record CashflowEntry(
        long Id,
        long AccountId,
        DateTime TransactionDate,
        decimal Balance,
        string? Note
        )
    {
    }
}

namespace CashMapper
{
    /// <summary>
    /// DTO for a category entry in the database. Used to categorize, track, and relate transactions to other application objects.
    /// </summary>
    internal record Category(
        long Id,
        string Name,
        string CategoryType
        )
    {
    }
}

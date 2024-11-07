namespace CashMapper.DataAccess.Entities;

/// <summary>
/// DTO for an entry that describes a balance recording of an account profile.
/// </summary>
internal record CashflowEntry: EntityBase
{
    public long AccountId { get; init; }
    public DateTime? EntryDate { get; init; }
    public decimal Balance { get; init; }
    public string? Note { get; init; }
}

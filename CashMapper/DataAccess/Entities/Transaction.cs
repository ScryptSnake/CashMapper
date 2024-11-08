namespace CashMapper.DataAccess.Entities;

/// <summary>
/// DTO for a transaction entry to catalogue budget transactions.
/// </summary>
public record Transaction : EntityBase
{
    public string? Description { get; init; }
    public string? Source { get; init; }
    public long CategoryId { get; init; }
    public string? Note { get; init; }
    public decimal Value { get; init; }
    public DateTime TransactionDate { get; }
}

namespace CashMapper.DataAccess.Entities;

/// <summary>
/// DTO for a transaction entry to catalogue budget transactions.
/// </summary>
internal record Transaction(
    long Id,
    string? Description,
    string? Source,
    DateTime TransactionDate,
    decimal Value,
    long CategoryId,
    string? Note
    )
{
}

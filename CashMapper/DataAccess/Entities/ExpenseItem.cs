namespace CashMapper.DataAccess.Entities;
/// <summary>
/// DTO for an item that represents a known monthly expense. 
/// </summary>
internal record ExpenseItem: EntityBase
{
    public string? Description { get; init; }
    public decimal MonthlyValue { get; init; }
    public long CategoryId { get; init; }
    public string? Note { get; init; }
}

namespace CashMapper.DataAccess.Entities;

/// <summary>
/// DTO for an item to be budgeted for. Example: 'Groceries'.
/// </summary>
public record BudgetItem : EntityBase
{
    public string? Description { get; init; }
    public decimal MonthlyValue { get; init; }
    public long CategoryId { get; init; }
    public string? Note { get; init; }

}

namespace CashMapper.DataAccess.Entities;

/// <summary>
/// DTO for an item to be budgeted for. Example: 'Groceries'.
/// </summary>
internal record BudgetItem : EntityBase
{
    public string? Description { get; init; }
    public decimal MonthlyValue { get; init; }
    public long Category { get; init; }
    public string? Note { get; init; }

}

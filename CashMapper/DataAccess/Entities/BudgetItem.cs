namespace CashMapper.DataAccess.Entities;

/// <summary>
/// DTO for an item to be budgeted for. Example: 'Groceries'.
/// </summary>
public record BudgetItem : EntityBase
{
    public string? Description { get; init; }

    private decimal monthlyValue;

    public decimal MonthlyValue
    {
        get => monthlyValue;
        init
        {
            if (value < 0)
                throw new ArgumentException("Invalid. Value less than zero.");
            monthlyValue = value;
        }
    }
    public long CategoryId { get; init; }
    public string? Note { get; init; }

}

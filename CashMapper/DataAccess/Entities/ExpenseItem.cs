namespace CashMapper.DataAccess.Entities;
/// <summary>
/// DTO for an item that represents a known monthly expense. 
/// </summary>
public record ExpenseItem: EntityBase
{
    public string? Description { get; init; }

    private decimal monthlyValue;
    public decimal MonthlyValue
    {
        get => monthlyValue;
        init
        {
            if(value > 0)
                throw new ArgumentException("Invalid. Expense value greater than zero.");
            monthlyValue = value;
        }
    }
    public long CategoryId { get; init; }
    public string? Note { get; init; }
}

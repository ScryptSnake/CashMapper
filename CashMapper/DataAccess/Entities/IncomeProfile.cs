namespace CashMapper.DataAccess.Entities;

/// <summary>
/// DTO for an account that generates an income.
/// </summary>
public record IncomeProfile : EntityBase
{
    public string Name { get; init; }
}

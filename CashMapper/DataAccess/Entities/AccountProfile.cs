namespace CashMapper.DataAccess.Entities;

/// <summary>
/// DTO for an account that holds a balance of money.
/// </summary>
internal record AccountProfile: EntityBase
{
    public string Name { get; init; }
}

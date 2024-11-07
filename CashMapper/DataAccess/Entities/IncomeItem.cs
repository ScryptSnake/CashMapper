namespace CashMapper.DataAccess.Entities;
/// <summary>
/// DTO for an entry that represents an income line-item associated with an IncomeProfile.
/// </summary>
internal record IncomeItem : EntityBase
{
    public string Name { get; init; }
    public long IncomeProfileId { get; init; }
    private decimal MonthlyValue { get; init; }
}

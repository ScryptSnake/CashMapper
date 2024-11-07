using CashMapper.Enums;

namespace CashMapper.DataAccess.Entities;

/// <summary>
/// DTO for a category entry in the database. Used to categorize, track, and relate transactions to other application objects.
/// </summary>
internal record Category: EntityBase 
{
    public string Name { get; init; }
    public CategoryTypes CategoryType {get; init; }

}

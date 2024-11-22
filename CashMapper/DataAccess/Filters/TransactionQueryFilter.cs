using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.DataAccess.Filters
{
    /// <summary>
    /// Used as a parameter to the TransactionRepository for more sophisticated querying.
    /// </summary>
    public record TransactionQueryFilter(
        long? CategoryId = null,
        string? DescriptionLike = null,
        string? source = null,
        string? NoteLike = null,
        string? Flag = null,
        (DateTimeOffset min, DateTimeOffset max)? DateRange = null,
        (int min, int max)? ValueRange = null)
    {

    }
}

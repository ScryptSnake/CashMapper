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
        string? CategoryName, 
        DateTimeOffset? StartTime, 
        DateTimeOffset EndTime, 
        string? DescriptionLike)
    {

    }
}

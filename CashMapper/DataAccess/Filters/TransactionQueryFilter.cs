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
        string? CategoryName = null,
        DateTime? StartTime=null,
        DateTime? EndTime=null,
        string? DescriptionLike = null,
        string? NoteLike = null,
        string? Flag = null,
        decimal? StartValue = null,
        decimal? EndValue = null)
    {

    }
}

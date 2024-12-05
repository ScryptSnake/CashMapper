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
        string? Source = null,
        string? NoteLike = null,
        string? Flag = null,
        (DateTimeOffset? min, DateTimeOffset? max)? DateRange = null,
        (decimal? min, decimal? max)? ValueRange = null) : IQueryFilter
    {
        public bool IsEmpty()
        {
            {
                return !CategoryId.HasValue &&
                        (DescriptionLike==null) &&
                        (Source==null) &&
                        (NoteLike==null) &&
                        (Flag==null) &&
                        (DateRange?.min.HasValue == false && DateRange?.max.HasValue == false) &&
                        (ValueRange?.min.HasValue == false && ValueRange?.max.HasValue == false);
            }

        }
    }
}

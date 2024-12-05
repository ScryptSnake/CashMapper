using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.DataAccess.Filters
{
    internal interface IQueryFilter
    {
        bool IsEmpty();
    }
}

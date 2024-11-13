using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.Enums
{
    /// <summary>
    ///  Defines the type of category:  Budget, Expense, Income
    /// </summary>
    public enum CategoryTypes
    {
        // The integer value is written to the database.
        // Dapper doesn't support type mappers for Enum. 
        // Could use a wrapper-type. 
        Undefined = 0,
        Budget = 1, 
        Expense = 2, 
        Income = 3,
    }
}

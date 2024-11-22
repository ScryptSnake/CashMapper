using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.Enums
{
    /// <summary>
    /// The operators used to evaluate a QueryFilter expression.
    /// </summary>
    public enum QueryOperators
    {
        Equals,
        NotEquals, 
        GreaterThan, 
        LessThan, 
        LessThanOrEqual,
        GreaterThanOrEqual,
        And,
        Or,
        Like,
        In,
        NotIn
    }
}

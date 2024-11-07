using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.DataAccess
{
    public sealed record DatabaseSettings()
    { 
        // Note: keep as parameter-less constructor for DI.
        public string Path { get; init; }
    }
}

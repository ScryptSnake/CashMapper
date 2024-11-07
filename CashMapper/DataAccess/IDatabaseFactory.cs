using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.DataAccess;


/// <summary>
/// A contract for an object that creates an IDatabase type.
/// </summary>
public interface IDatabaseFactory
{
    public Task<IDatabase> GetDatabase();
}


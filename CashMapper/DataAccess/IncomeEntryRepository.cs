using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.Entities;


namespace CashMapper.DataAccess;

public class IncomeEntryRepository: IRepository<IncomeEntry>
{

    private IDatabase Database { get; }


    public IncomeEntryRepository(IDatabase database) //this would be the factory instead. Not the DB instance.
    {
        Database = database;

    }

    public Task<bool> ExistsAsync(IncomeEntry entity)
    {
        
    }

    public Task<IncomeEntry> FindAsync(IncomeEntry entity)
    {
        throw new NotImplementedException();
    }

    public Task<IncomeEntry> GetAsync(IncomeEntry entity)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<IncomeEntry>> GetMultipleAsync<TFilter>(TFilter filter)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<IncomeEntry>> AddAsync(IncomeEntry entity)
    {
        throw new NotImplementedException();
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.Entities;


namespace CashMapper.DataAccess;

public class IncomeItemRepository: IRepository<IncomeItem>
{

    private Task<IDatabase> DatabaseTask { get; }


    public IncomeItemRepository(IDatabaseFactory databaseFactory) //this would be the factory instead. Not the DB instance.
    {
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(IncomeItem entity)
    {
        var db = await DatabaseTask;

    }

    public async Task<IncomeItem> FindAsync(IncomeItem entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IncomeItem> GetAsync(IncomeItem entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IncomeItem>> GetMultipleAsync<TFilter>(TFilter filter)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IncomeItem>> AddAsync(IncomeItem entity)
    {
        throw new NotImplementedException();
    }
}

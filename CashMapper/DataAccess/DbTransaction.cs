using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.DataAccess
{

    /// <summary>
    /// Wraps an IDbTransaction type. 
    /// The purpose of this object is to hide the Connection from consumers of the database and still allow transact-ability.
    /// </summary>
    public sealed class DbTransaction
    {
        private IDbTransaction Transaction { get; }

        public DbTransaction(IDbTransaction transaction)
        {
            Transaction = transaction;
        }
        public void Commit() => Transaction.Commit();
        public void Rollback() => Transaction.Rollback();
    }
}

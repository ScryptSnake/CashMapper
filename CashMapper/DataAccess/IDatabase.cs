using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.DataAccess
{
    /// <summary>
    /// Defines a contract for a Database to be used with this application.
    /// </summary>
    public interface IDatabase: IDisposable, IAsyncDisposable
    {
        /// <summary>Performs the specified action with the connection. </summary>
        void Perform(Action<IDbConnection> action);

        /// <summary>Performs the specified action with the connection and transaction. </summary>
        void Perform(Action<IDbConnection,IDbTransaction> action);

        /// <summary>Performs the specified function with the connection and type argument. </summary>
        void Perform<TArg>(Action<IDbConnection,TArg> func, TArg argument);

        /// <summary>Performs the specified function with the connection, transaction, and type argument. </summary>
        void Perform<TArg>(Action<IDbConnection, TArg, IDbTransaction> func, TArg argument);

        // Async PERFORM Overloads.
        Task PerformAsync(Func<IDbConnection, Task> func);
        Task PerformAsync<TArg>(Func<IDbConnection, TArg, Task> func, TArg argument);
        Task PerformAsync<TArg>(Func<IDbConnection, TArg, IDbTransaction, Task> func, TArg argument);



        // Explanation:  method 'Get' returns a type TResult
        // Get must accept function that takes an argument of type IDbConnection,
        //the calling method must return a type of TResult, which could be any type.

        /// <summary>Returns a value by specifying a function which uses the connection and return type. </summary>
        TResult Get<TResult>(Func<IDbConnection, TResult> func);

        /// <summary>Returns a value by specifying a function which uses the connection, transaction and return type. </summary>
        TResult Get<TResult>(Func<IDbConnection, IDbTransaction, TResult> func);
        /// <summary>Returns a value by specifying a function which takes an argument, uses the connection and return type. </summary>
        TResult Get<TResult, TArg>(Func<IDbConnection, TArg, TResult> func, TArg argument);

        /// <summary>Returns a value by specifying a function which takes an argument, uses the connection, transaction, and return type. </summary>
        TResult Get<TResult, TArg>(Func<IDbConnection, TArg, IDbTransaction, TResult> func, TArg argument);


        // Async GET Overloads.
        Task<TResult> GetAsync<TResult>(Func<IDbConnection, Task<TResult>> func);
        Task<TResult> GetAsync<TResult>(Func<IDbConnection, IDbTransaction, Task<TResult>> func);
        //GetAsync with argument: 
        Task<TResult> GetAsync<TResult, TArg>(Func<IDbConnection, TArg, Task<TResult>> func, TArg argument);
        Task<TResult> GetAsync<TResult, TArg>(Func<IDbConnection, TArg, IDbTransaction, Task<TResult>> func, TArg argument);


    }
}

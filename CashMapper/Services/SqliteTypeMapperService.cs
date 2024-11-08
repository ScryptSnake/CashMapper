using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.TypeHandlers;
using Dapper;

namespace CashMapper.Services
{
    /// <summary>
    /// Registers various type handlers for dapper/sqlite to property load/write .NET data types.
    /// </summary>
    internal static class SqliteTypeMapperService
    {
        public static void RegisterTypeMappers()
        {
            Console.WriteLine("REGISTERING!");
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new DateTimeOffsetTypeHandler());
            SqlMapper.AddTypeHandler(new CategoryTypesTypeHandler());
        }
    }
}

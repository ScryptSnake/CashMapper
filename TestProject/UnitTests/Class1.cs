using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.Filters;
using CashMapper.Enums;
using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Data.Sqlite;

namespace TestProject.UnitTests
{
    /// <summary>
    ///  This class tests the QueryBuilder class, which dynamically assembles a SELECT query.
    ///  Queries are validated against an in-memory SQLite db using dapper.
    /// </summary>
    public class QueryBuilderTests
    {

        private SqliteConnection Connection { get; }

        public QueryBuilderTests()
        {
            // Setup a connection to a temp database
            Connection = new SqliteConnection("Data Source=:memory:");

            Connection.Open();
            // Create a test table.
            Connection.Execute(@"CREATE TABLE products
                                (
                                    id                  INTEGER PRIMARY KEY,
                                    name                TEXT,
                                    price               NUMERIC,
                                    date                TEXT,
                                    category            TEXT      
                                );");

        }

        [Fact]
        public void QueryBuilderTests_AddCriteria_ShouldMatchExpected()
        {
            // Arrange.
            const string EXPECTED_SQL = "WHERE id = @id;";
            dynamic expectedObj = new { id = 5 };

            var builder = new QueryBuilder();
            builder.AddCriteria("id", 5, QueryOperators.Equals);

            // Act.
            var outputSql = builder.BuildWhereClause();
            var outputObj = builder.GetParameter();

            //Console.WriteLine(outputObj.Id);

            //foreach (var property in outputObj.GetType().GetProperties())
            //{
            //    var propertyName = property.Name;
            //    var propertyValue = property.GetValue(outputObj);

            //    Console.WriteLine($"......................{propertyName}: {propertyValue}");
            //}



            // Assert.
            Assert.Equal(EXPECTED_SQL,outputSql);
            outputObj.Should().BeEquivalentTo(expectedObj);

        }


    }
}

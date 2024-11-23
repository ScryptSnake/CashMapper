using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CashMapper.DataAccess.Filters;
using CashMapper.Enums;
using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace TestProject.IntegrationTests.Backend
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
        public void QueryBuilderTests_AddCriteria_Then_BuildWhereClause_Then_GetParameter_ShouldMatchExpected()
        {
            // Arrange.
            const string EXPECTED_SQL = "WHERE id = @id;";
            var expectedObj = new { id = 5 };

            var builder = new QueryBuilder();

            // Act.
            builder.AddCriteria("id", 5, QueryOperators.Equals);
            var outputSql = builder.BuildWhereClause();
            var outputObj = builder.GetParameter();
            var areEqual = ObjectsAreEqual(outputObj, expectedObj);

            // Assert.
            Assert.Equal(EXPECTED_SQL, outputSql);
            Assert.True(ObjectsAreEqual(expectedObj, outputObj));
        }

        [Fact]
        public void QueryBuilderTests_AddCriteria_And_AddMultiCriteria_Then_BuildWhereClause_Then_GetParameter_ShouldMatchExpected()
        {
            // Arrange.
            // Keep string single line!
            const string EXPECTED_SQL = "WHERE id = @id AND name NOT IN @name AND category <> @category AND price > @price1 AND price < @price2;";
            var expectedObj = new
            {
                id = 1,
                name = new[] { "product1", "product2" },
                category = "products",
                price1 = 20,
                price2 = 5
            };

            var builder = new QueryBuilder();

            // Act.
            builder.AddCriteria("id", expectedObj.id, QueryOperators.Equals);
            builder.AddMultiCriteria("name", expectedObj.name, QueryOperators.NotIn);
            builder.AddCriteria("category", expectedObj.category, QueryOperators.NotEquals);
            builder.AddRangeCriteria("price", (expectedObj.price1, expectedObj.price2), null, false);

            var outputSql = builder.BuildWhereClause();
            var outputObj = builder.GetParameter();
            var areEqual = ObjectsAreEqual(outputObj, expectedObj);

            // Assert.
            Assert.Equal(EXPECTED_SQL, outputSql);
            Assert.True(ObjectsAreEqual(expectedObj, outputObj));
        }


        /// <summary>
        /// FluentAssertions has issues with comparing an expando object from QueryBuilder.GetParameter()
        /// This method serializes each object and compares their serialization.
        /// Ultimately, the true test is whether the .GetParameter() object is valid to dapper as a command param.
        /// </summary>
        public bool ObjectsAreEqual(object a, object b)
        {
            // Checks if two objects contain same values and properties
            if (a == null && b == null) return true; // Both are null
            if (a == null || b == null) return false; // One is null

            // Serialize both objects
            var jsonA = JsonSerializer.Serialize(a, new JsonSerializerOptions { WriteIndented = false });
            var jsonB = JsonSerializer.Serialize(b, new JsonSerializerOptions { WriteIndented = false });

            Console.WriteLine(jsonA);
            Console.WriteLine("---------------------------------------");
            Console.WriteLine(jsonB);

            // Compare the serialized strings
            return jsonA == jsonB;
        }

    }
}

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.Entities;
using CashMapper.Enums;

namespace CashMapper.DataAccess.TypeHandlers;
/// <summary>
/// Allows SQLite to handle the .NET CategoryTypes enum type for I/O operations. 
/// </summary>
public sealed class CategoryTypesTypeHandler : SqlMapper.TypeHandler<CategoryTypes>
{
    public override void SetValue(IDbDataParameter parameter, CategoryTypes value)
    {
        parameter.Value = value.ToString();
    }
    public override CategoryTypes Parse(object value)
    {
        if (value is string stringValue)
        {
            return (CategoryTypes)Enum.Parse(typeof(CategoryTypes), stringValue, true);
        }
        throw new NotSupportedException("Unsupported CategoryTypes parse type returned from database.");
    }
}

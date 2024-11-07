using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.DataAccess.TypeHandlers;
/// <summary>
/// Allows SQLite to handle the .NET DateTime type for I/O operations. 
/// </summary>
public sealed class DateTimeOffsetTypeHandler : SqlMapper.TypeHandler<DateTimeOffset>
{
    public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
    {
        parameter.Value = value.UtcDateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
    }
    public override DateTimeOffset Parse(object value)
    {
        if (value is string stringValue)
        {
            return DateTimeOffset.Parse(stringValue);
        }
        throw new NotSupportedException("Unsupported DateTimeOffset parse type returned from database.");
    }
}
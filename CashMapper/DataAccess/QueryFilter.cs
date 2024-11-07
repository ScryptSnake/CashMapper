using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.DataAccess;

/// <summary>
/// An object that holds conditions for filtering a database table. 
/// </summary>
public class QueryFilter()
{
    private StringBuilder Builder { get; } = new StringBuilder();
    private Dictionary<string, string> Values { get; } = new Dictionary<string, string>();

    /// <summary>
    /// Adds the conditional.
    /// </summary>
    /// <param name="field">The field name to filter.</param>
    /// <param name="valueOperator">The operator for the field and value, such as '=', '>', '<'..</param>
    /// <param name="value">The value.</param>
    /// <param name="logicalOperator">A logical operator appended to the end of the parameter string, such as 'AND', 'OR' .</param>
    /// <returns></returns>
    public void AddConditional(string field, string valueOperator, string value, string logicalOperator = "")
    {
        if(IsEmpty()) logicalOperator = ""; //Ignore any logical operator passed.
        Builder.AppendFormat(" {0} {1} @{2} {3}", field, valueOperator, field, logicalOperator);

        // Add value to dictionary.
        Values.Add(field, value);   
    }

    /// <summary>
    /// Gets the parameter object to be passed to a database execution/query method. 
    /// </summary>
    /// <returns>An object containing filter properties with their values. For passing to a database execution method.</returns>
    public object GetParameter()
    {
        var obj = new ExpandoObject() as IDictionary<string, object>;

        foreach (var kvp in Values)
        {
            obj.Add(kvp.Key, kvp.Value);
        }
        return obj;
    }


    public bool IsEmpty()
    {
        if (string.IsNullOrEmpty(Builder.ToString())) return true;
        return false;
    }

    public override string ToString()
    {
        return Builder.ToString();
    }

}

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CashMapper.Enums;

namespace CashMapper.DataAccess;

/// <summary>
/// An object that dynamically assembles WHERE criteria for a SQL query. 
/// </summary>
internal class QueryFilter()
{
    private StringBuilder Builder { get; } = new StringBuilder();
    private Dictionary<string, string> Values { get; } = new Dictionary<string, string>();

    /// <summary>
    /// Adds the conditional.
    /// </summary>
    /// <param name="field">The field name to filter.</param>
    /// <param name="value">The value as a string for the assembled SQL string.</param>
    /// <param name="evaluator">The operator for the field and value, such as '=', '>', '<'..</param>
    /// <param name="logicalOperator">A logical operator appended to the end of the parameter string, such as 'AND', 'OR' .</param>
    /// <returns></returns>
    public void AddCriteria(string field, string value, QueryOperators evaluator, QueryOperators? logicalOperator = null)
    {
        // Ignore logicalOperator if no criteria has been added.
        if (IsEmpty()) logicalOperator = null;

        // Validate logicalOperator. Acceptable values are:  .AND, .OR
        if (logicalOperator != null)
        {
            if (logicalOperator != QueryOperators.And && logicalOperator != QueryOperators.Or)
            {
                throw new ArgumentException("Logical operator not supported. Accepts: AND, OR");
            }
        }

        // Parse operators to a string representation.
        var evaluatorString = ParseQueryOperators(evaluator);
        var logicalString = ParseQueryOperators(logicalOperator);

        // Validate the field for any injection risks.
        if (!ValidateString(field))
            throw new ArgumentException("Invalid field name provided.");

        // Append a formatted string to the builder. 
        Builder.AppendFormat(" {0} {1} @{2} {3}", field, evaluatorString, field, logicalString);

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

    /// <summary>
    /// Parses a QueryOperators type to a string representation. 
    /// </summary>
    private static string ParseQueryOperators(QueryOperators? value)
    {
        switch(value)
        {
            case null: return string.Empty;
            case QueryOperators.And: return "AND";
            case QueryOperators.Or: return "OR";
            case QueryOperators.Equals: return "=";
            case QueryOperators.NotEquals: return "!=";
            case QueryOperators.GreaterThan: return ">";
            case QueryOperators.LessThan: return "<";
            case QueryOperators.LessThanOrEqual: return "<=";
            case QueryOperators.GreaterThanOrEqual: return ">=";
            default:
                throw new ArgumentException("Failed to parse. Value not found.");

        }
}
    private static bool ValidateString(string input)
    {
        // A level of protection against any SQL injection attack.
        // This also ensures that fields are valid C# identifier.
        // Therefore, dot notation for field names will not work with this class.
        // Ensure is single word, no whitespace.
        // Only allows underscore special char. 
        if (input.Any(char.IsWhiteSpace) || 
            (Regex.IsMatch(input, "^[a-zA-Z0-9_\\-]+$\r\n"))) return false;
        return true;
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

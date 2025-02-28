﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CashMapper.Enums;
using System.Runtime.CompilerServices;

namespace CashMapper.DataAccess.Filters;

/// <summary>
/// An object that dynamically assembles WHERE criteria for a SQL query. 
/// </summary>
/// 
internal class QueryBuilder()
{
    private StringBuilder Builder { get; } = new StringBuilder();
    private Dictionary<string, object?> Values { get; } = new Dictionary<string, object?>();

    /// <summary>
    /// Adds two criteria to test whether a field's value is within two values. 
    /// </summary>
    /// <param name="field">The field name.</param>
    /// <param name="valueRange">A ValueTuple describing the range. If null, criteria is not added.</param>
    /// <param name="inclusive"> When true, the start and end value are included in the range.</param>
    /// <param name="paramName">This is a prefix in which an integer is appended and incremented for each value.
    /// When null, defaults to field name.</param>
    public void AddRangeCriteria(string field, (object? min, object? max)? valueRange, string? paramName = null, bool inclusive=true)
    {
        if (paramName == null) paramName = field;
        if (valueRange == null) return;
        if (inclusive)
        {
            AddCriteria(field, valueRange.Value.min, 
                QueryOperators.GreaterThanOrEqual,
                QueryOperators.And,paramName + "1");
            AddCriteria(field, valueRange.Value.max,
                QueryOperators.LessThanOrEqual,
                QueryOperators.And, paramName + "2");
        }
        else
        {
            AddCriteria(field, valueRange.Value.min,
                QueryOperators.GreaterThan,
                QueryOperators.And, paramName + "1");
            AddCriteria(field, valueRange.Value.max,
                QueryOperators.LessThan,
                QueryOperators.And, paramName + "2");
        }
    }

    /// <summary>
    /// Tests whether a field is 'IN' or 'NOT IN' a list of values.
    /// Values that are null or empty string are included.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <param name="startValue">The start value.</param>
    /// <param name="endValue">The end value.</param>
    /// <param name="inclusive">if set to <c>true</c> [inclusive].</param>
    /// <param name="evaluator"> Only 'Equals' and 'NotEquals' are valid for this method. </param>
    /// <param name="paramName">The SQL param name used in the resulting SQL string
    /// When null, defaults to field name.</param>
    /// <returns></returns>
    public void AddMultiCriteria(string field, IEnumerable<object> values,
                                QueryOperators evaluator=QueryOperators.Equals,
                                QueryOperators logicalOperator=QueryOperators.And, 
                                string? paramName = null)
    {
        // Ensure field name is valid.
        if (!FieldIsValid(field))
            throw new ArgumentException("Invalid field name provided.");

        // Default paramName to field if not provided.
        // Validate the field name, ensure does not exist.
        if (paramName == null) paramName = field;
        if (!FieldIsValid(paramName) || Values.ContainsKey(paramName))
        {
            throw new ArgumentException("Invalid param name provided.");
        }

        // Check evaluator is valid: IN or NOT IN.
        if (evaluator != QueryOperators.In && evaluator != QueryOperators.NotIn)
        {
            throw new ArgumentException("Invalid evaluator passed. Accepts: 'NotEquals' or 'Equals'.");
        }

        // Parse operators to a string representation.
        var evaluatorString = ParseQueryOperators(evaluator);
        var logicalString = ParseQueryOperators(logicalOperator);

        // Write to builder.
        Builder.AppendFormat("{0} {1} {2} @{3} ", logicalString, field, evaluatorString, paramName);

        // Write value to dict.
        Values.Add(paramName,values);
    }

    /// <summary>
    /// Adds the conditional.
    /// </summary>
    /// <param name="field">The field name to filter.</param>
    /// <param name="value">The value as a string for the assembled SQL string.</param>
    /// <param name="evaluator">The operator to test the value, such as '=', '>', '<'..</param>
    /// <param name="logicalOperator">A logical operator appended to the beginning of the criteria string, such as 'AND', 'OR' .</param>
    /// <param name="ignoreIfEmptyOrNull"> When true, values that are null or are empty string are ignored. Thus, no criteria is added.</param>
    /// <param name="paramName">The sqlite parameter name, must be a unique value. When null, defaults to the field name.</param>
    public void AddCriteria(string field, object? value, QueryOperators evaluator,
        QueryOperators? logicalOperator = QueryOperators.And, string? paramName = null, bool ignoreIfEmptyOrNull = true)
    {
        // Ensure field name is valid.
        if (!FieldIsValid(field))
            throw new ArgumentException("Invalid field name provided.");

        // Check parameter name is valid and wasn't already added, auto assign if null. 
        if (paramName == null) paramName = field;
        if (!FieldIsValid(paramName) || Values.ContainsKey(paramName))
        {
            throw new ArgumentException("Invalid param name provided.");
        }

        // Check if a value was passed, ignore accordingly.
        if (value is null || value.ToString() == string.Empty)
        {
            if (ignoreIfEmptyOrNull) return;
        }

        // Ignore logicalOperator if no criteria has been added to the object.
        if (IsEmpty()) logicalOperator = null;

        // Validate logicalOperator. Acceptable values are:  .AND, .OR
        if (logicalOperator != null &&
            logicalOperator != QueryOperators.And &&
            logicalOperator != QueryOperators.Or)
        {
            throw new ArgumentException("Logical operator not supported. Accepts: AND, OR");
        }

        // Parse operators to a string representation.
        var evaluatorString = ParseQueryOperators(evaluator);
        var logicalString = ParseQueryOperators(logicalOperator);

        // Add percent signs to value if a 'LIKE' operator is used.
        if (evaluator == QueryOperators.Like) value = $"%{value}%";

        // Append a formatted string with ADO parameter syntax to the builder. 
        Builder.AppendFormat("{0} {1} {2} @{3} ", logicalString, field, evaluatorString, paramName);

        // Add value to dictionary.
        Values.Add(paramName, value);
    }

    /// <summary>
    /// Gets the parameter object to be passed to a database execution/query method. 
    /// </summary>
    /// <returns>An object containing filter properties with their values. For passing to a database execution method.</returns>
    public object GetParameter()
    {
        dynamic obj = new ExpandoObject();
        var objDictionary = obj as IDictionary<string, object>;

        foreach (var kvp in Values)
        {
            objDictionary.Add(kvp.Key, kvp.Value);
        }
        return obj;
    }

    /// <summary>
    /// Builds the WHERE SQL clause for the object.
    /// </summary>
    public string BuildWhereClause(bool trailingSemicolon = true)
    {
        if (Values.Count==0) throw new DataException("Unable to build WHERE clause. Criteria is empty.");
        var output = $"WHERE {Builder.ToString().Trim()}";
        if (trailingSemicolon) output = output + ";";
        return output;
    }

    /// <summary>
    /// Parses a QueryOperators type to a string representation. 
    /// </summary>
    private static string ParseQueryOperators(QueryOperators? value)
    {
        switch (value)
        {
            case null: return string.Empty;
            case QueryOperators.And: return "AND";
            case QueryOperators.Or: return "OR";
            case QueryOperators.Equals: return "=";
            case QueryOperators.NotEquals: return "<>";
            case QueryOperators.GreaterThan: return ">";
            case QueryOperators.LessThan: return "<";
            case QueryOperators.LessThanOrEqual: return "<=";
            case QueryOperators.GreaterThanOrEqual: return ">=";
            case QueryOperators.Like: return "LIKE";
            case QueryOperators.In: return "IN";
            case QueryOperators.NotIn: return "NOT IN";
            default:
                throw new ArgumentException("Failed to parse. Value not found.");
        }
    }
    private static bool FieldIsValid(string input)
    {
        // A 'possible' level of protection against any SQL injection attack.
        // This also ensures that fields are valid C# identifier.
        // Therefore, dot notation for field names will not work with this class.
        // Ensure is single word, no whitespace.
        // Only allows underscore special char. 
        if (string.IsNullOrEmpty(input)) return false;
        if (input.Any(char.IsWhiteSpace) ||
            Regex.IsMatch(input, "^[a-zA-Z0-9_\\-]+$\r\n")) return false;
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

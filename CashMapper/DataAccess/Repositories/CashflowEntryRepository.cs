﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.Entities;
using Dapper;


namespace CashMapper.DataAccess.Repositories;

/// <summary>
/// Provides methods for performing CRUD operations on the cashflow_entries table. 
/// </summary>
public class CashflowEntryRepository : IRepository<CashflowEntry>
{
    private Task<IDatabase> DatabaseTask { get; }

    public CashflowEntryRepository(IDatabaseFactory databaseFactory){
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM cashflow_entries WHERE id=@Id;";
        var count = await db.ExecuteScalarAsync<long>(SQL, new {Id=id});
        switch (count)
        {
            case 0: return false;
            case 1: return true;
            default:
                throw new DataException(
                    $"""
                     Database returned an invalid number of records.
                      Expected 1 or 0. Actual: {count}
                     """);
        }
        return false;
    }

    public async Task<CashflowEntry?> FindAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT id, account_id, date AS entry_date,
                            CAST(balance AS REAL) AS value, note, date_modified, flag
                           FROM cashflow_entries WHERE id=@id;";
        var entity = await db.GetAsync<CashflowEntry>(SQL, new { id });
        return entity;
    }

    public async Task<CashflowEntry> GetAsync(CashflowEntry entity)
    {
        var result = await FindAsync(entity.Id);
        return result ?? throw new DataException("Provided entity does not exist.");
    }

    public async Task<IEnumerable<CashflowEntry>> GetAllAsync()
    {
        const string sql = @$"SELECT id, account_id, date AS entry_date, CAST(balance AS REAL) AS value, note
                    date_created, date_modified, flag 
                    FROM cashflow_entries;";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<CashflowEntry>(sql);
    }

    public async Task<CashflowEntry> AddAsync(CashflowEntry entity)
    {
        // Note:  DateCreated and DateModified fields default to current timestamp inside backend.
        const string SQL = @$"INSERT INTO cashflow_entries(account_id, date, balance, note, flag)
                            VALUES(@AccountId, @EntryDate, @Balance, @Note, @Flag);
                            SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(SQL, entity);
        return await GetAsync(entity with { Id = id });
    }

    public async Task<CashflowEntry> UpdateAsync(CashflowEntry entity)
    {
        if (entity.Id == default) throw new InvalidDataException("CashflowEntry Id field not provided.");
        var sql = $@"UPDATE cashflow_entries,
                  SET account_id=@AccountId, date=@EntryDate, balance=@Balance, note=@Note,
                  flag=@Flag,
                  date_modified='{DateTimeOffset.Now.UtcDateTime.ToString("s", CultureInfo.InvariantCulture)}'
                  WHERE id=@Id;";
        var db = await DatabaseTask;
        await db.ExecuteAsync(sql, entity);
        return await GetAsync(entity);
    }
}

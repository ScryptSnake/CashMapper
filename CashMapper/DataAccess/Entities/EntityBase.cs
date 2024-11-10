using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.DataAccess.Entities;
/// <summary>
/// A base class for which all DTO entities inherit. 
/// </summary>
public abstract record EntityBase
{
    public long Id { get; init; }
    public DateTime DateCreated { get; init;}
    public DateTime DateModified {get; init;}
    public string? Flag { get; init; }

}

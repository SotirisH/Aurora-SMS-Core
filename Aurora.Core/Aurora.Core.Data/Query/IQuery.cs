using System;
using System.Collections.Generic;
using System.Text;

namespace Aurora.Core.Data.Query
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <remarks>https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=92</remarks>
    public interface IQuery<TResult>
    {
    }

    public interface IQueryExecutor<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult GetResult(TQuery query);
    }
}

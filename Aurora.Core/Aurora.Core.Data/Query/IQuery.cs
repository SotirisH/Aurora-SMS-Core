using System;
using System.Collections.Generic;
using System.Text;

namespace Aurora.Core.Data.Query
{
    public interface IQuery<TResult>
    {
    }

    public interface IQueryExecutor<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult GetResult(TQuery query);
    }
}

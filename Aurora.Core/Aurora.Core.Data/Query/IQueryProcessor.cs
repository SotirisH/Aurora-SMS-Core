using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Aurora.Core.Data.Query
{
    public interface IQueryProcessor
    {
        TResult Process<TResult>(IQuery<TResult> query);
    }
    //public sealed class QueryProcessor : IQueryProcessor
    //{
    //    private readonly Container container;

    //    public QueryProcessor(Container container)
    //    {
    //        this.container = container;
    //    }

    //    [DebuggerStepThrough]
    //    public TResult Process<TResult>(IQuery<TResult> query)
    //    {
    //        var handlerType =
    //            typeof(IQueryExecutor<,>).MakeGenericType(query.GetType(), typeof(TResult));

    //        dynamic handler = container.GetInstance(handlerType);

    //        return handler.GetResult((dynamic)query);
    //    }
    //}
}

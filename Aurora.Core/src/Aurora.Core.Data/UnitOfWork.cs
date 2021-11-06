using System;
using System.Collections.Generic;
using System.Transactions;
using Aurora.Core.Data.Abstractions;

namespace Aurora.Core.Data
{
    /// <summary>
    ///     A unit of work pattern that supports more than one context
    ///     and commits them all in one transaction
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HashSet<ISupportsUnitOfWork> _contexts = new();

        public void AddContext(ISupportsUnitOfWork context)
        {
            _contexts.Add(context ?? throw new ArgumentNullException("context"));
        }

        public void RemoveContext(ISupportsUnitOfWork context)
        {
            _contexts.Remove(context ?? throw new ArgumentNullException("context"));
        }

        /// <summary>
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        public void CommitAll(bool acceptAllChangesOnSuccess = true)
        {
            using (var trn = new TransactionScope())
            {
                foreach (ISupportsUnitOfWork context in _contexts)
                {
                    context.SaveChanges(acceptAllChangesOnSuccess);
                }

                trn.Complete();
            }
        }
    }
}

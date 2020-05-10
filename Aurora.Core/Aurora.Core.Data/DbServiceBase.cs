using System;

namespace Aurora.Core.Data
{
    public abstract class DbServiceBase<DB> where DB : AuditableDbContext
    {
        protected DbServiceBase(DB auditableDbContext)
        {
            DbContext = auditableDbContext ?? throw new ArgumentNullException("auditableDbContext");
        }

        protected DB DbContext { get; }
    }
}

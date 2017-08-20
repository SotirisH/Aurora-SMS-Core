using System;
using System.Collections.Generic;
 using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Core.Data
{

    public abstract class DbServiceBase<DB> where DB : AuditableDbContext
    {
        protected DB DbContext { get; private set; }


        protected DbServiceBase(DB auditableDbContext)
        {
            DbContext = auditableDbContext ?? throw new ArgumentNullException("auditableDbContext");
        }


    }
}

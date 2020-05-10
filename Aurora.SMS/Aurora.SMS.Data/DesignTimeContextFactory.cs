using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Aurora.SMS.Data
{
    /// <summary>
    ///     An inplementation of the IDesignTimeDbContextFactory
    ///     is needed to run Add-Migration
    /// </summary>
    /// <remarks>https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext</remarks>
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<SMSDb>
    {
        public SMSDb CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SMSDb>();
            return new SMSDb();
        }
    }
}

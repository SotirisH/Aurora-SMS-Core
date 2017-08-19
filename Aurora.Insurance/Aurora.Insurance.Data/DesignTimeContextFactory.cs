using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurora.Insurance.Data
{
    /// <summary>
    /// An inplementation of the IDesignTimeDbContextFactory
    /// is needed to run Add-Migration
    /// </summary>
    /// <remarks>https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext</remarks>
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<InsuranceDb>
    {
        public InsuranceDb CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<InsuranceDb>();
            return new InsuranceDb();
        }
    }
}

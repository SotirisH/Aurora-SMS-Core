using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Aurora.Core.Data.Abstractions;

namespace Aurora.Core.Data.Tests
{
    class AuditableDbContextMock: AuditableDbContext
    {
        public AuditableDbContextMock(DbContextOptions options,
            ICurrentUserService currentUserService) :base(options, currentUserService)
        {
           
        }
        public DbSet<MockEntity> MockEntity { get; set; }
    }
}

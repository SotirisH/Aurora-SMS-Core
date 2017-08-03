using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Aurora.Core.Data.Tests
{
    [TestClass]
    public class AuditableDbContextTests
    {
        [TestMethod]
        public void TestSaveChanges()
        {
            //// In-memory database only exists while the connection is open
            //using (var connection = new SqliteConnection(ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString))
            //{
            //    connection.Open();
            //    var options = new DbContextOptionsBuilder<AuditableDbContextMock>()
            //           .UseSqlite(connection)
            //           .Options;
            //    // Create the schema in the database
            //    using (var context = new AuditableDbContextMock(options))
            //    {
            //        context.Database.EnsureCreated();
            //    }
            //    connection.Close();
            //}

            var mockICurrentUserService = new Moq.Mock<ICurrentUserService>();
            mockICurrentUserService.Setup(m => m.GetCurrentUser()).Returns("TestUser");
            var cnString = @"Server=.\SQL16;Database=Test;Trusted_Connection=True";
            var options = new DbContextOptionsBuilder<AuditableDbContextMock>()
                    .UseSqlServer(cnString)
                    .Options;
            // Create the schema in the database
            using (var context = new AuditableDbContextMock(options, mockICurrentUserService.Object))
            {
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }
            using (var context = new AuditableDbContextMock(options, mockICurrentUserService.Object))
            {
                var mock = new MockEntity();
                mock.Description = "dddddddddddddddddddddddddddddddddddddddddddddd";
                context.MockEntity.Add(mock);
                context.Commit();
            }
        }
    }
}

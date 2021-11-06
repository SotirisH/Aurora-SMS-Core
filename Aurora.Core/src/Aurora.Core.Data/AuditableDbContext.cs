using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aurora.Core.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Aurora.Core.Data
{
    /// <summary>
    ///     Extended DbContext that adds timestamps on the EntityBase classes when they are saved
    /// </summary>
    public abstract class AuditableDbContext : DbContext, ISupportsUnitOfWork
    {
        private readonly ICurrentUserService _currentUserService;

        protected AuditableDbContext()
        {
            _currentUserService = new DefaultCurrentUserService();
        }

        protected AuditableDbContext(DbContextOptions options,
            ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        /// <summary>
        ///     Validates all models, adds tracking record and saves the changes
        ///     This is called last when we save the changes
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ValidateModel();
            AddAudit();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ValidateModel();
            AddAudit();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        ///     All entities are being audited.
        ///     This overload should be used instead of the original one
        /// </summary>
        /// <returns></returns>
        private void AddAudit()
        {
            string currentUser = _currentUserService.GetCurrentUser();
            IEnumerable<EntityEntry<AuditableEntityBase>> changeSet = ChangeTracker.Entries<AuditableEntityBase>()?.ToList();

            if (changeSet == null)
            {
                return;
            }

            foreach (EntityEntry<AuditableEntityBase> entry in changeSet.Where(c => c.State == EntityState.Added))
            {
                entry.Entity.CreatedOn = DateTime.Now;
                entry.Entity.CreatedBy = currentUser;
            }

            foreach (EntityEntry<AuditableEntityBase> entry in changeSet.Where(c => c.State == EntityState.Modified))
            {
                entry.Entity.ModifiedOn = DateTime.Now;
                entry.Entity.ModifiedBy = currentUser;
            }
        }

        /// <summary>
        ///     Validated the model.
        ///     An ValidationException is thrown if the validation doesn't pass
        /// </summary>
        /// <remarks>
        ///     https://blogs.msmvps.com/ricardoperes/2016/04/25/implementing-missing-features-in-entity-framework-core-part-3/
        /// </remarks>
        public void ValidateModel()
        {
            var serviceProvider = this.GetService<IServiceProvider>();
            var items = new Dictionary<object, object>();
            var errorResults = new List<ValidationResult>();
            foreach (EntityEntry entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                object entity = entry.Entity;
                //var context = new ValidationContext(entity, serviceProvider, items);
                var context = new ValidationContext(entity);
                var results = new List<ValidationResult>();

                if (Validator.TryValidateObject(entity, context, results, true) == false)
                {
                    foreach (ValidationResult result in results)
                    {
                        if (result != ValidationResult.Success)
                        {
                            throw new ValidationException(result, null, result.ErrorMessage);
                        }
                    }
                }
            }
        }
    }
}

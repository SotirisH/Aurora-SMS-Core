using System;
using System.ComponentModel.DataAnnotations;

namespace Aurora.Core.Data.Abstractions
{
    /// <summary>
    ///     Base class for all EF Class with audit tracking fields & Timestamp
    /// </summary>
    /// <remarks>
    /// </remarks>
    public abstract class AuditableEntityBase
    {
        /// <summary>
        ///     The user name of the user who perform the insert
        /// </summary>
        [MaxLength(50)]
        public string CreatedBy { get; set; } = null!;

        // http://stackoverflow.com/questions/27038524/sql-column-default-value-with-entity-framework
        // https://andy.mehalick.com/2014/02/06/ef6-adding-a-created-datetime-column-automatically-with-code-first-migrations/
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        ///     The user name of the user who perform the modification
        /// </summary>
        [MaxLength(50)]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using Aurora.Core.Data;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public abstract class Contact : EntityBase
    {
        public Guid ContactId { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; } = null!;
        public string? FatherName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string TaxId { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public Address? Address { get; set; }
        public virtual ICollection<Phone>? Phones { get; set; } = new List<Phone>();
        public Guid OrganizationId { get; set; }

        /// <summary>
        ///     Standard reference to the organization
        /// </summary>
        public virtual Organization Organization { get; set; } = null!;

        /// <summary>
        ///     The user that represents this entity
        /// </summary>
        public Guid? UserId { get; set; }
    }
}

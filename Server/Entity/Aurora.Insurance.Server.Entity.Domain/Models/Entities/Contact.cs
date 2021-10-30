using System;
using System.Collections.Generic;
using Aurora.Core.Data;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public abstract class Contact:EntityBase
    {
        public Guid ContactId { get; set; }
        public string TaxId { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public Address? Address { get; set; }
        public virtual ICollection<Phone>? Phones { get; set; }
    }
}

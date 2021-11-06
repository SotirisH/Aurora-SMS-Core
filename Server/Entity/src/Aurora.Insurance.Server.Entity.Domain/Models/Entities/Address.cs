using System;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public string AddressLine { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public Guid CountryId { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; } = null!;
    }
}

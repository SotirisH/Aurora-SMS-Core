using System;
using Aurora.Insurance.Server.Entity.Domain.Enums;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public class Address
    {
        public string AddressLine { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}

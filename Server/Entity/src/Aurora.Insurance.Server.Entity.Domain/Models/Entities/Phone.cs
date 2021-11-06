using System;
using Aurora.Insurance.Server.Entity.Domain.Enums;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public class Phone
    {
        public Guid PhoneId { get; set; }
        public string Number { get; set; } = null!;
        public PhoneType PhoneType { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using Aurora.Insurance.Server.Entity.Domain.Models.Entities;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Dtos
{
    public record Broker
    {
        public Guid AgentId => ContactId;
        public Guid ContactId { get; init; }
        public string LastName { get; init; } = null!;
        public string TaxId { get; init; } = null!;
        public string EmailAddress { get; init; } = null!;
    }
}

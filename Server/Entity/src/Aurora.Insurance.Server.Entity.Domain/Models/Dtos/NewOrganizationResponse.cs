using System;
using Aurora.Insurance.Server.Entity.Domain.Models.Entities;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Dtos
{
    public record NewOrganizationResponse
    {
        public Guid OrganizationId { get; init; }
        public Broker Broker { get; init; }
    }
}

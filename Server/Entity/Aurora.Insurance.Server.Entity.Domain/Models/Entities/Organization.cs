using System;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public class Organization
    {
        public Guid OrganizationId { get; set; }

        /// <summary>
        ///     The default agent ID that is linked with the organization
        /// </summary>
        public Guid BrokerId { get; set; }

        /// <summary>
        ///     The default agent that is linked with the organization
        /// </summary>
        public Agent Broker { get; set; }
    }
}

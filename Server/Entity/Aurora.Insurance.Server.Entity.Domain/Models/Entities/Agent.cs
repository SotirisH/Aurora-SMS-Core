using System;
using System.Collections.Generic;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public class Agent : Contact
    {
        public Guid AgentId { get; set; }

        /// <summary>
        ///     SubAgents in the hierarchy
        /// </summary>
        public ICollection<Agent>? Agents { get; set; }

        public bool IsBroker { get; set; }
    }
}

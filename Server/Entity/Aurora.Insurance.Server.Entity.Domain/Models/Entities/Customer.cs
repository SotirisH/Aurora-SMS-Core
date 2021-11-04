using System;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public class Customer : Contact
    {
        public DrivingLicence DrivingLicence { get; set; } = null!;
        public Guid AgentId { get; set; }

        /// <summary>
        ///     The agent that this customer belongs
        /// </summary>
        public virtual Agent Agent { get; set; } = null!;
    }
}

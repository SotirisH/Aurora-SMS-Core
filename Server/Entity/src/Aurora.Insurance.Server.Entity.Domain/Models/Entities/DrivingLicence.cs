using System;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public class DrivingLicence
    {
        public Guid DrivingLicenceId { get; set; }
        public Guid CountryId { get; set; }
        public string Number { get; set; } = null!;
        public string? Version { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string? Conditions { get; set; }

        public Guid ContactId { get; set; }
        public Customer Customer { get; set; } = null!;
    }
}

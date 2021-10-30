using System;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public class Customer : Contact
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? FatherName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string DrivingLicence { get; set; } = null!;
    }
}

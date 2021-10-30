using System;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public class Agent : Contact
    {
        public Guid BrokerId { get; set; }
    }
}

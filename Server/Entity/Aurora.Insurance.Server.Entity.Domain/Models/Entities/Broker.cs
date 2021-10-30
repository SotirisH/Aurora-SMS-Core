using System;
using System.Collections.Generic;

namespace Aurora.Insurance.Server.Entity.Domain.Models.Entities
{
    public class Broker : Contact
    {
        public Guid BrokerId { get; set; }
        public string Title { get; set; } = null!;
        
    }
}

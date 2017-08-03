using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aurora.Core.Data.Tests
{
   public  class MockEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(5)]
        public string Description { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}

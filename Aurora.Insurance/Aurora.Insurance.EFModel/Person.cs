﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Insurance.EFModel
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string DrivingLicenceNum { get; set; }
        public string TaxId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}

using Aurora.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Insurance.EFModel
{
    /// <summary>
    /// Registered insurance companies in the system
    /// </summary>
    public class Company:EntityBase
    {

        /// <summary>
        /// The widely known code of the company
        /// </summary>
        public string Id { get; set; }
        public string Description { get; set; }
    }
}

using Aurora.Insurance.Services.DTO;
using System.Collections.Generic;

namespace Aurora.SMS.Web.Models
{
    public class CustomerSelectionViewModel
    {
        public QueryCriteriaDTO Criteria { get; set; }
        public IEnumerable<Insurance.EFModel.Company> Companies { get; set; }
        public IEnumerable<Aurora.Insurance.Services.DTO.ContractDTO> Contracts { get; set; }
    }
}
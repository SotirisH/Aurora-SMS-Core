using System.Threading.Tasks;
using Aurora.Insurance.Server.Entity.Domain.Interfaces.Application;
using Aurora.Insurance.Server.Entity.Domain.Models.Dtos;
using Aurora.Insurance.Server.Entity.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aurora.Insurance.Server.Entity.Api.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationsController : Controller
    {
        private readonly IOrganizationServices _organizationServices;

        public OrganizationsController(IOrganizationServices organizationServices)
        {
            _organizationServices = organizationServices;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<NewOrganizationResponse>> Post([FromBody] NewOrganizationRequest value)
        {
            NewOrganizationResponse result = await _organizationServices.CreateOne(value);
            return Ok(result);
        }
    }
}

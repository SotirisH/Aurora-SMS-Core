using System.Collections.Generic;
using System.Threading.Tasks;
using Aurora.Insurance.EFModel;
using Aurora.Insurance.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aurora.Insurance.WebAPI.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyServices _companyServices;

        public CompaniesController(ICompanyServices companyServices)
        {
            _companyServices = companyServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> Get()
        {
            return Ok(await _companyServices.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> Get(string id)
        {
            Company resource = await _companyServices.GetOne(id);
            if (resource != null)
            {
                return Ok(resource);
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Company>> Post([FromBody] Company value)
        {
            Company result = await _companyServices.CreateOne(value);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id,
            [FromBody] Company value)
        {
            Company result = await _companyServices.UpdateOne(value);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _companyServices.DeleteOne(id);
            return NoContent();
        }
    }
}

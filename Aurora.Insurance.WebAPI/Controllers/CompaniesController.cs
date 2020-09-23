using Aurora.Insurance.EFModel;
using Aurora.Insurance.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aurora.Insurance.WebAPI.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyServices companyServices;

        public CompaniesController(ICompanyServices companyServices)
        {
            this.companyServices = companyServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> Get()
        {
            return Ok(await companyServices.GetAll());
        }

        [HttpGet("{id}")]
        public Company Get(string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public void Post([FromBody] Company value)
        {
        }

        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Company value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }
    }
}
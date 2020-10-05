﻿using Aurora.Insurance.EFModel;
using Aurora.Insurance.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aurora.Insurance.WebAPI.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonServices _personServices;

        public PersonsController(IPersonServices personServices)
        {
            this._personServices = personServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> Get()
        {
            return Ok(await _personServices.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> Get(int id)
        {
            var resource = await _personServices.GetOne(id);
            if (resource != null)
            {
                return Ok(resource);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Company>> Post([FromBody] Person value)
        {
            var result = await _personServices.CreateOne(value);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Person value)
        {
            var result = await _personServices.UpdateOne(value);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _personServices.DeleteOne(id);
            return NoContent();
        }
    }
}
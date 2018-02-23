using Aurora.SMS.Service;
using Aurora.SMS.Web.Areas.Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Aurora.SMS.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/SmsTemplates")]
    public class SmsTemplatesController : Controller
    {
        private readonly ITemplateServices _templateServices;
        private readonly IMapper _mapper;
        /// <summary>
        /// Sample Web API
        /// </summary>
        /// <param name="templateServices"></param>
        /// <remarks>Noramlly the _UoW.Commit should be invoked at the end of the bussiness transaction</remarks>
        public SmsTemplatesController(ITemplateServices templateServices,
                                    IMapper mapper)
        {
            _templateServices = templateServices;
        }

        // GET: api/SmsTemplates
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _templateServices.GetAll();
            if (result == null || !result.Any())
            {
                return NoContent();
            }
            var model = _mapper.Map<IEnumerable<SmsTemplateModel>>(result);
            return new ObjectResult(model);
        }

        // GET: api/SmsTemplates/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var result = _templateServices.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<SmsTemplateModel>(result);
            //returns 200 with a JSON response body
            return new ObjectResult(model);
        }

        // POST: api/SmsTemplates

        /// <summary>
        /// Updates an existing template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]  SmsTemplateModel model)
        {
            TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrWhiteSpace(model.RowVersion))
            {
                return BadRequest("This method is not allowed to update an entity. Please use the PUT verb");
            }

            _templateServices.CreateTemplate(_mapper.Map<EFModel.Template>(model));
            return Ok();
            //TODO: return CreatedAtRoute("GetTodo", new { id = item.Id }, item);

        }

        // PUT: api/SmsTemplates/5
        [HttpPut("{id}")]
        public IActionResult Put(SmsTemplateModel model)
        {
            TryValidateModel(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(model.RowVersion))
            {
                return BadRequest("This method is not allowed to Create an entity. Please use the POST verb");
            }

            _templateServices.Update(_mapper.Map<EFModel.Template>(model));
            return new NoContentResult();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _templateServices.DeleteTemplate(id);
            return new NoContentResult();
        }
    }
}

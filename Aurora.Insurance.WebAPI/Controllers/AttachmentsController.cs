using Aurora.Insurance.EFModel;
using Aurora.Insurance.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Aurora.Insurance.WebAPI.Controllers
{
    [Route("api/{endpoint}/{entityId:int}/[controller]")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentServices attachmentServices;

        public AttachmentsController(IAttachmentServices attachmentServices)
        {
            this.attachmentServices = attachmentServices;
        }

        [HttpPost("{filename}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Attachment>> Post(string endpoint,
            int entityId,
            string fileName,
            IFormFile file)
        {
            var savedAttachment = await attachmentServices.CreateOne(new Attachment
            {
                Title= fileName,
                FileName = fileName,
                ContentLength = file.Length,
                MimeType = file.ContentType,
            }, file.OpenReadStream());
            return Ok(savedAttachment);
        }
    }
}
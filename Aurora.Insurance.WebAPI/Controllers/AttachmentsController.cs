using System.Collections.Generic;
using System.Threading.Tasks;
using Aurora.Insurance.EFModel;
using Aurora.Insurance.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        ///     Uploads a file into the server and creates an attachment record
        /// </summary>
        /// <param name="endpoint">The name of the resource that this attachment will be under</param>
        /// <param name="entityId">The Id of the parent resource</param>
        /// <param name="fileName">The desired file name</param>
        /// <param name="title">The title of the attachment</param>
        /// <param name="description"></param>
        /// <param name="type">The type of the document</param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("{filename}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Attachment>> Post(string endpoint,
            int entityId,
            string fileName,
            [FromForm] string title,
            [FromForm] string description,
            [FromForm] string type,
            [FromForm] IFormFile file)
        {
            Attachment savedAttachment = await attachmentServices.CreateOne(new Attachment
                {
                    Title = string.IsNullOrWhiteSpace(title) ? fileName : title,
                    Description = description,
                    Type = type,
                    FileName = fileName,
                    ContentLength = file.Length,
                    MimeType = file.ContentType
                },
                file.OpenReadStream());
            return Ok(savedAttachment);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attachment>>> Get()
        {
            return Ok(await attachmentServices.GetAll());
        }
    }
}

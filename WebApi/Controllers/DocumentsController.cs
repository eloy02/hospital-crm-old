using Core.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApi.DB;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private TokenList AuthTokens;
        private DBServise DB = new DBServise();

        public DocumentsController(TokenList authTokens)
        {
            AuthTokens = authTokens;
        }

        [HttpGet]
        public async Task<ActionResult<Document>> GetDocumentForPacient(Guid? token, int? pacientId)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            if (pacientId == null)
                return BadRequest();

            var raw = await DB.GetDocumentByPacientAsync(pacientId.Value);

            if (raw == null)
            {
                return NotFound();
            }
            else
            {
                var data = new Document().Assign(raw);

                return new ActionResult<Document>(data);
            }
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Document>>> GetDocuments(Guid? token)
        //{
        //    if (token == null)
        //        return Unauthorized();

        //    if (token.HasValue && !AuthTokens.Contains(token.Value))
        //        return Unauthorized();

        //    var rawData = await DB.GetAllDocumentsAsync();

        //    if (rawData == null)
        //    {
        //        return NoContent();
        //    }
        //    else
        //    {
        //        var data = rawData.Select(d => new Document().Assign(d));

        //        return new ActionResult<IEnumerable<Document>>(data);
        //    }
        //}

        [HttpPut]
        public async Task<ActionResult> UpdateDocumentForPacient(Guid? token, int? pacientId, Document value)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            if (pacientId == null)
                return BadRequest();

            if (value == null)
                return BadRequest();

            await DB.UpdatePacientDocumentAsync(value.Content.ToArray(), pacientId.Value);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> SaveDocumentForPacient(Guid? token, int? pacientId, [FromBody] Document document)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            if (pacientId == null)
                return BadRequest();

            if (document == null)
                return BadRequest();

            var doc = new Documents().Assign(document);

            doc.PacientId = pacientId;

            await DB.SaveDocumentForPacient(doc);

            return Ok();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DB;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private DBServise DB = new DBServise();

        // GET: api/Documents
        [HttpGet("{pacient}")]
        public async Task<ActionResult<byte[]>> GetDocumentForPacient([FromBody] Pacients pacient)
        {
            var data = await DB.GetDocumentByPacientAsync(pacient);

            if (data == null)
            {
                return NotFound();
            }
            else return new ActionResult<byte[]>(data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Documents>>> GetDocuments()
        {
            var data = await DB.GetAllDocumentsAsync();

            if (data == null)
            {
                return NoContent();
            }
            else return new ActionResult<IEnumerable<Documents>>(data);
        }

        // POST: api/Documents
        [HttpPut("{pacientId}")]
        public async Task UpdateDocumentForPacient(int pacientId, [FromBody] byte[] value)
        {
            await DB.UpdatePacientDocumentAsync(value, pacientId);
        }
    }
}
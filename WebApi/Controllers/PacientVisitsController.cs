using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DB;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientVisitsController : ControllerBase
    {
        private DBServise DB = new DBServise();

        // GET: api/VisitLogs
        [HttpGet("{pacientId}")]
        public async Task<ActionResult<IEnumerable<VisitLogs>>> GetVisitLogs(int pacientId)
        {
            var data = await DB.GetVisitLogForPacient(pacientId);

            if (data == null)
            {
                return NotFound();
            }
            else return new ActionResult<IEnumerable<VisitLogs>>(data);
        }

        // POST: api/VisitLogs
        [HttpPost]
        public async Task Post([FromBody] VisitLogs value)
        {
            await DB.SetPacientVisitAsync(value);
        }
    }
}
using Core.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DB;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientVisitsController : ControllerBase
    {
        private TokenList AuthTokens;
        private DBServise DB = new DBServise();

        public PacientVisitsController(TokenList authTokens)
        {
            AuthTokens = authTokens;
        }

        // GET: api/VisitLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitLog>>> GetVisitLogs(Guid? token, int? pacientId)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            if (pacientId.HasValue == false)
                return BadRequest();

            var rawdata = await DB.GetVisitLogForPacient(pacientId.Value);

            if (rawdata == null)
            {
                return NotFound();
            }
            else
            {
                var data = rawdata.Select(r => new VisitLog().Assign(r));

                return new ActionResult<IEnumerable<VisitLog>>(data);
            }
        }

        // POST: api/VisitLogs
        [HttpPost]
        public async Task<ActionResult> SaveVisit(Guid? token, [FromBody] VisitLog value)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            if (value == null)
            {
                return BadRequest();
            }
            else
            {
                var val = new VisitLogs().Assign(value);

                await DB.SetPacientVisitAsync(val);

                return Ok();
            }
        }
    }
}
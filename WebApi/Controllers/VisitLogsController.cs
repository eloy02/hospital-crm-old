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
    [ApiController]
    public class VisitLogsController : ControllerBase
    {
        private TokenList AuthTokens;
        private DBServise DB = new DBServise();

        public VisitLogsController(TokenList authTokens)
        {
            AuthTokens = authTokens;
        }

        // GET: api/VisitLogs
        [HttpGet]
        [Route("api/[controller]/pacientsvisitlogs")]
        public async Task<ActionResult<IEnumerable<VisitLog>>> GetVisitLogs(Guid? token, int? pacientId)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            if (pacientId.HasValue == false)
                return BadRequest("pacientId is required");

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

        [HttpGet]
        [Route("api/[controller]/doctorsvisitlogs")]
        public async Task<ActionResult<IEnumerable<VisitLog>>> GetVisitLogsForDoctor(Guid? token, int? doctorId)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            if (doctorId.HasValue == false)
                return BadRequest("Doctor Id is Required");

            var rawdata = await DB.GetVisitLogForDoctor(doctorId.Value);

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
        [Route("api/[controller]")]
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
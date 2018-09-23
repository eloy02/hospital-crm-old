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
    public class PacientsController : ControllerBase
    {
        private TokenList AuthTokens;
        private DBServise DB = new DBServise();

        public PacientsController(TokenList authTokens)
        {
            AuthTokens = authTokens;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pacient>>> GetPacients(Guid? token)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            var rawdata = await DB.GetPacientsAsync();

            if (rawdata == null)
            {
                return NotFound();
            }
            else
            {
                var data = rawdata.Select(p => new Pacient().Assign(p));

                ActionResult<IEnumerable<Pacient>> result = new ActionResult<IEnumerable<Pacient>>(data);

                return result;
            }
        }

        [HttpPost]
        public async Task<ActionResult> SavePacient(Guid? token, [FromBody] Pacient value)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            if (value == null)
                return BadRequest();
            else
            {
                var val = new Pacients().Assign(value);

                await DB.SavePacientAsync(val);

                return Ok();
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePacient(Guid? token, int? id, [FromBody] Pacient value)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            if (value == null)
                return BadRequest();
            else
            {
                var val = new Pacients().Assign(value);

                await DB.UpdatePacientDataAsync(val);

                return Ok();
            }
        }
    }
}
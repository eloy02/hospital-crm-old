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
    public class CommonDataController : ControllerBase
    {
        private DBServise DB = new DBServise();
        private TokenList AuthTokens;

        public CommonDataController(TokenList tokens)
        {
            AuthTokens = tokens;
        }

        // GET: api/CommonData
        [HttpGet]
        [Route("api/[controller]/doctors")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors(Guid? token)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            var rawData = await DB.GetDoctorsAsync();
            var data = rawData.Select(r => new Doctor().Assign(r));

            if (data == null)
                return NotFound();
            else return new ActionResult<IEnumerable<Doctor>>(data);
        }

        [HttpPost]
        [Route("api/[controller]/doctors")]
        public async Task<ActionResult> AddDoctorAsync(Guid? token, [FromBody] Doctor doc)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            if (doc == null)
                return BadRequest("Указаны не все параметры");
            else
            {
                var docdb = new Doctors().Assign(doc);

                if (await DB.AddDoctorAsync(docdb))
                    return Ok();
                else
                    return BadRequest("Ошибка");
            }
        }

        [HttpPost]
        [Route("api/[controller]/users")]
        public async Task<ActionResult> AddUserAsync(string programmGuid, [FromBody] Core.Types.User user, string password)
        {
            if (string.IsNullOrEmpty(programmGuid))
                return Unauthorized();

            if (!await DB.CheckProgrammGuid(programmGuid))
                return Unauthorized();

            if (await DB.CheckProgrammGuid(programmGuid) && user != null)
            {
                var userdb = new Models.User().Assign(user);
                await DB.AddUserAsync(userdb, password);

                return Ok();
            }
            else
                return BadRequest("Указаны не все параметры");
        }

        [HttpGet]
        [Route("api/[controller]/users")]
        public async Task<ActionResult<IEnumerable<Core.Types.User>>> GetUsersAsync(string programmGuid)
        {
            if (string.IsNullOrEmpty(programmGuid))
                return Unauthorized();
            else if (await DB.CheckProgrammGuid(programmGuid))
            {
                var raw = await DB.GetUsersAsync();

                if (raw != null)
                {
                    var data = raw.Select(u => new Core.Types.User().Assign(u)).ToList();

                    return data;
                }
                else
                    return NoContent();
            }
            else return Unauthorized();
        }
    }
}
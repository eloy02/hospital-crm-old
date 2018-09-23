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
    }
}
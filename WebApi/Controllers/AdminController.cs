using Core.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using WebApi.DB;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        private TokenList AuthTokens;
        private DBServise DB = new DBServise();

        public AdminController(TokenList tokens)
        {
            AuthTokens = tokens;
        }

        [HttpPost]
        [Route("api/[controller]/doctors")]
        public async Task<ActionResult<Doctor>> AddDoctorAsync(Guid? token, [FromBody] Doctor doc)
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

                var r = await DB.AddDoctorAsync(docdb);

                return new Doctor().Assign(r);
            }
        }

        [HttpPut]
        [Route("api/[controller]/doctors")]
        public async Task<ActionResult<Doctor>> UpdateDoctorAsync(Guid? token, [FromBody]Doctor doc)
        {
            try
            {
                if (token == null)
                    return Unauthorized();

                if (token.HasValue && !AuthTokens.Contains(token.Value))
                    return Unauthorized();

                if (doc == null)
                    return BadRequest("Указаны не все параметры");

                var docdb = new Doctors().Assign(doc);

                var r = await DB.UpdateDoctorAsync(docdb);

                return new Doctor().Assign(r);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete]
        [Route("api/[controller]/doctors")]
        public async Task<ActionResult> DeleteDoctorAsync(Guid? token, [FromBody]Doctor doc)
        {
            try
            {
                if (token == null)
                    return Unauthorized();

                if (token.HasValue && !AuthTokens.Contains(token.Value))
                    return Unauthorized();

                if (doc == null)
                    return BadRequest("Указаны не все параметры");

                var docdb = new Doctors().Assign(doc);

                await DB.DeleteDoctorAsync(docdb);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("api/[controller]/users")]
        public async Task<ActionResult<Core.Types.User>> AddUserAsync(Guid? token, [FromBody] Core.Types.User user, string password)
        {
            try
            {
                if (token == null)
                    return Unauthorized();

                if (token.HasValue && !AuthTokens.Contains(token.Value))
                    return Unauthorized();

                if (user != null && !string.IsNullOrEmpty(password))
                {
                    var userdb = new Models.User().Assign(user);
                    var r = await DB.AddUserAsync(userdb, password);

                    return new Core.Types.User().Assign(r);
                }
                else
                    return BadRequest("Указаны не все параметры");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut]
        [Route("api/[controller]/users")]
        public async Task<ActionResult<Core.Types.User>> UpdateUserAsync(Guid? token, [FromBody] Core.Types.User user)
        {
            try
            {
                if (token == null)
                    return Unauthorized();

                if (token.HasValue && !AuthTokens.Contains(token.Value))
                    return Unauthorized();

                if (user is null)
                    return BadRequest("Указаны не все параметры");

                var userdb = new Models.User().Assign(user);
                var r = await DB.UpdateUserAsync(userdb);

                return new Core.Types.User().Assign(r);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        // PUT: api/Admin/5
        [HttpDelete]
        [Route("api/[controller]/users")]
        public async Task<ActionResult> DeleteUserAsync(Guid? token, int? id)
        {
            try
            {
                if (token == null)
                    return Unauthorized();

                if (token.HasValue && !AuthTokens.Contains(token.Value))
                    return Unauthorized();

                if (id is null)
                    return BadRequest("Указаны не все параметры");

                await DB.DeleteUserAsync(id.Value);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
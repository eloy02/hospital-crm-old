using Core.Types.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApi.DB;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    public class AuthenticatorController : ControllerBase
    {
        private TokenList AuthTokens;
        private DBServise DB = new DBServise();

        public AuthenticatorController(TokenList authTokens)
        {
            AuthTokens = authTokens;
        }

        // GET: api/Authenticator
        [HttpGet]
        [Route("api/[controller]/login")]
        public async Task<ActionResult<AuthResult>> LogInUserAsync([FromQuery] int userId, [FromQuery]string programmGuid, [FromQuery]string password)
        {
            if (string.IsNullOrEmpty(programmGuid))
                return BadRequest("Не все данные заполнены");

            if (string.IsNullOrEmpty(password))
                return BadRequest("Не все данные заполнены");

            var checkRes = await DB.CheckProgrammGuid(programmGuid);

            if (checkRes == true)
            {
                if (await DB.CheckUserAsync(userId, password))
                {
                    var user = await DB.GetUsersAsync(userId);

                    var token = Guid.NewGuid();

                    AuthTokens.Add(token);

                    return new ActionResult<AuthResult>(new AuthResult { SessionGuid = token, UserAccess = user.AccessGroup });
                }
                else return Unauthorized();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<ActionResult<Guid>> GetTokenAsync(string programmGuid)
        {
            if (string.IsNullOrEmpty(programmGuid))
                return BadRequest("Не все данные заполнены");

            var checkRes = await DB.CheckProgrammGuid(programmGuid);

            if (checkRes == true)
            {
                var token = Guid.NewGuid();

                AuthTokens.Add(token);

                return new ActionResult<Guid>(token);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("api/[controller]/test")]
        public ActionResult<Guid> GetTestToken()
        {
            return new ActionResult<Guid>(Guid.NewGuid());
        }

        [HttpDelete]
        [Route("api/[controller]")]
        public ActionResult DeleteToken(Guid? token)
        {
            if (token == null)
                return Unauthorized();

            if (token.HasValue && !AuthTokens.Contains(token.Value))
                return Unauthorized();

            if (token.HasValue && AuthTokens.Contains(token.Value))
            {
                AuthTokens.Remove(token.Value);

                return Ok();
            }
            else return BadRequest();
        }
    }
}
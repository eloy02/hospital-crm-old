using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DB;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonDataController : ControllerBase
    {
        private DBServise DB = new DBServise();

        // GET: api/CommonData
        [HttpGet(Name = "Doctors")]
        public async Task<ActionResult<IEnumerable<Doctors>>> GetDoctors()
        {
            var data = await DB.GetDoctorsAsync();

            if (data == null)
                return NotFound();
            else return new ActionResult<IEnumerable<Doctors>>(data);
        }
    }
}
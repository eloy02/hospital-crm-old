using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DB;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientsController : ControllerBase
    {
        private DBServise DB = new DBServise();

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pacients>>> GetPacients()
        {
            var data = await DB.GetPacientsAsync();

            ActionResult<IEnumerable<Pacients>> result = new ActionResult<IEnumerable<Pacients>>(data);

            if (data == null)
            {
                return NotFound();
            }
            else return result;
        }

        [HttpPost]
        public async Task SavePacient([FromBody] Pacients value)
        {
            await DB.SavePacientAsync(value);
        }

        [HttpPut("{id}")]
        public async Task UpdatePacient(int id, [FromBody] Pacients value)
        {
            await DB.UpdatePacientDataAsync(value);
        }
    }
}
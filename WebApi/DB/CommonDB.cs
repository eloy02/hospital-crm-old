using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.DB
{
    public partial class DBServise
    {
        public async Task<IEnumerable<Doctors>> GetDoctorsAsync()
        {
            using (var db = new HospitalContext())
            {
                var raw = await db.Doctors.ToListAsync();

                return raw;
            }
        }
    }
}
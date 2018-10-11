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

        public async Task<bool> AddDoctorAsync(Doctors doc)
        {
            using (var db = new HospitalContext())
            {
                doc.Id = 0;

                db.Doctors.Add(doc);
                await db.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> CheckProgrammGuid(string guid)
        {
            using (var db = new HospitalContext())
            {
                var raw = await db.ProgrammGUID.SingleOrDefaultAsync(g => g.GUID.Equals(guid));

                if (raw != null)
                {
                    return true;
                }
                else return false;
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.DB
{
    public partial class DBServise
    {
        public async Task SetPacientVisitAsync(VisitLogs visit)
        {
            using (var db = new HospitalContext())
            {
                db.VisitLogs.Add(visit);

                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<VisitLogs>> GetVisitLogForPacient(int pacientId)
        {
            using (var db = new HospitalContext())
            {
                var raw = await db.VisitLogs.Where(v => v.PacientId == pacientId).ToListAsync();

                foreach (var r in raw)
                {
                    r.Pacient = await db.Pacients.FindAsync(r.PacientId);
                    r.Doctor = await db.Doctors.FindAsync(r.DoctorId);
                }

                return raw;
            }
        }
    }
}
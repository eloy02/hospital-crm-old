using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DB.EF
{
    public class DbServise
    {
        public async Task SavePacientAsync(Pacients data)
        {
            using (var db = new HospitalContext())
            {
                data.Id = default(int);
                db.Pacients.Add(data);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Pacients>> GetPacientsAsync()
        {
            using (var db = new HospitalContext())
            {
                var raw = await db.Pacients.ToListAsync();

                return raw;
            }
        }

        public async Task<byte[]> GetDocumentByPacientAsync(Pacients pacient)
        {
            using (var db = new HospitalContext())
            {
                var raw = await db.Documents.Where(p => p.PacientId == pacient.Id).ToListAsync();

                if (raw != null)
                {
                    return raw.SingleOrDefault().Document;
                }
                else return null;
            }
        }

        public async Task<IEnumerable<Doctors>> GetDoctorsAsync()
        {
            using (var db = new HospitalContext())
            {
                var raw = await db.Doctors.ToListAsync();

                return raw;
            }
        }

        public async Task SetPacientVisitAsync(VisitLogs visit)
        {
            using (var db = new HospitalContext())
            {
                db.VisitLogs.Add(visit);

                await db.SaveChangesAsync();
            }
        }
    }
}
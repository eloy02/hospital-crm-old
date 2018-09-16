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
                var raw = await db.Pacients.Where(p => p.Id == pacient.Id).ToListAsync();

                //if (raw != null)
                //{
                //    return raw.Documents.Where(p => p.Id == pacient.Id).SingleOrDefault().Document;
                //}
                //else return null;

                return null;
            }
        }
    }
}
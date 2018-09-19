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

        public async Task UpdatePacientDocumentAsync(byte[] documentByte, Pacients pacient)
        {
            using (var db = new HospitalContext())
            {
                var doc = await db.Documents.SingleOrDefaultAsync(d => d.PacientId == pacient.Id);

                if (doc != null)
                {
                    doc.Document = documentByte;

                    await db.SaveChangesAsync();
                }
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

        public async Task UpdatePacientDataAsync(Pacients pacient)
        {
            using (var db = new HospitalContext())
            {
                var pac = await db.Pacients.FindAsync(pacient.Id);

                if (pac != null)
                {
                    pac.BuildingNumber = pacient.BuildingNumber;
                    pac.FirstName = pacient.FirstName;
                    pac.FlatNumber = pacient.FlatNumber;
                    pac.LastName = pacient.LastName;
                    pac.PacientPhoneNumber = pacient.PacientPhoneNumber;
                    pac.PacientType = pacient.PacientType;
                    pac.ParentFirstName = pacient.ParentFirstName;
                    pac.ParentLastName = pacient.ParentLastName;
                    pac.ParentPatronymicName = pacient.ParentPatronymicName;
                    pac.ParentPhoneNumber = pacient.ParentPhoneNumber;
                    pac.PatronymicName = pacient.PatronymicName;
                    pac.Sity = pacient.Sity;
                    pac.Street = pacient.Street;

                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
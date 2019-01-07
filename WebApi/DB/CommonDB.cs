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

        public async Task<Doctors> AddDoctorAsync(Doctors doc)
        {
            using (var db = new HospitalContext())
            {
                doc.Id = 0;

                var r = db.Doctors.Add(doc);
                await db.SaveChangesAsync();

                return r.Entity;
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

        public async Task<Doctors> UpdateDoctorAsync(Doctors doc)
        {
            using (var db = new HospitalContext())
            {
                var docDb = await db.Doctors.SingleOrDefaultAsync(d => d.Id == doc.Id);

                if (docDb != null)
                {
                    docDb.LastName = doc.LastName;
                    docDb.PatronymicName = doc.PatronymicName;
                    docDb.Position = doc.Position;
                    docDb.FirstName = doc.FirstName;
                    docDb.IsActive = doc.IsActive;

                    await db.SaveChangesAsync();

                    return docDb;
                }
                else return null;
            }
        }

        public async Task DeleteDoctorAsync(Doctors doc)
        {
            using (var db = new HospitalContext())
            {
                var docDb = await db.Doctors.SingleOrDefaultAsync(d => d.Id == doc.Id);

                if (docDb != null)
                {
                    db.Doctors.Remove(docDb);

                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
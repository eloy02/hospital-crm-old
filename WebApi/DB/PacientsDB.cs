using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.DB
{
    public partial class DBServise
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
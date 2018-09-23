using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.DB
{
    public partial class DBServise
    {
        public async Task<Documents> GetDocumentByPacientAsync(int pacientId)
        {
            using (var db = new HospitalContext())
            {
                var raw = await db.Documents.Where(p => p.PacientId == pacientId).ToListAsync();

                if (raw != null)
                {
                    return raw.SingleOrDefault();
                }
                else return null;
            }
        }

        public async Task UpdatePacientDocumentAsync(byte[] documentByte, int pacientId)
        {
            using (var db = new HospitalContext())
            {
                var doc = await db.Documents.SingleOrDefaultAsync(d => d.PacientId == pacientId);

                if (doc != null)
                {
                    doc.Document = documentByte;

                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Documents>> GetAllDocumentsAsync()
        {
            using (var db = new HospitalContext())
            {
                var raw = await db.Documents.ToListAsync();

                return raw;
            }
        }

        public async Task SaveDocumentForPacient(Documents value)
        {
            using (var db = new HospitalContext())
            {
                db.Documents.Add(value);

                await db.SaveChangesAsync();
            }
        }
    }
}
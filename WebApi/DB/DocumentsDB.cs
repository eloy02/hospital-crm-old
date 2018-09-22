using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.DB
{
    public partial class DBServise
    {
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
    }
}
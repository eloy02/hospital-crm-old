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
    }
}
using DB.NetCore.Model;
using Microsoft.EntityFrameworkCore;

namespace DB.NetCore
{
    public class HospitalContext : DbContext
    {
        public DbSet<Pacient> Pacients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<VisitLog> VisitLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=192.168.88.210;Initial Catalog=Hospital;User ID=sa;Password=D4!QyE;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
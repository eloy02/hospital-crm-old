using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebApi.Models
{
    public partial class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Doctors> Doctors { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<Pacients> Pacients { get; set; }
        public virtual DbSet<VisitLogs> VisitLogs { get; set; }
        public virtual DbSet<ProgrammGUID> ProgrammGUID { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json");
                var config = builder.Build();
                string connectionString = config.GetConnectionString("HospitalDatabase");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgrammGUID>(entity =>
            {
                entity.HasIndex(e => e.Id);
            });

            modelBuilder.Entity<Documents>(entity =>
            {
                entity.HasIndex(e => e.PacientId)
                    .HasName("IX_PacientId");

                entity.HasOne(d => d.Pacient)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.PacientId)
                    .HasConstraintName("FK_dbo.Documents_dbo.Pacients_PacientId");
            });

            modelBuilder.Entity<Pacients>(entity =>
            {
                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.Sity).IsRequired();

                entity.Property(e => e.Street).IsRequired();
            });

            modelBuilder.Entity<VisitLogs>(entity =>
            {
                entity.HasIndex(e => e.DoctorId)
                    .HasName("IX_DoctorId");

                entity.HasIndex(e => e.PacientId)
                    .HasName("IX_PacientId");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.VisitLogs)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_dbo.VisitLogs_dbo.Doctors_DoctorId");

                entity.HasOne(d => d.Pacient)
                    .WithMany(p => p.VisitLogs)
                    .HasForeignKey(d => d.PacientId)
                    .HasConstraintName("FK_dbo.VisitLogs_dbo.Pacients_PacientId");
            });
        }
    }
}
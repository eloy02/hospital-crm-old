using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=rehabilitationchentre.czvxsxk4tzy6.us-east-1.rds.amazonaws.com;Initial Catalog=Hospital;Persist Security Info=True;User ID=eloy;Password=D4!QyErt;App=EntityFramework");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

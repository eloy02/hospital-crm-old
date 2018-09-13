namespace DB.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HospitalContext : DbContext
    {
        public HospitalContext()
            : base("name=HospitalContext")
        {
        }

        public virtual DbSet<Doctors> Doctors { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<Pacients> Pacients { get; set; }
        public virtual DbSet<VisitLogs> VisitLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctors>()
                .HasMany(e => e.VisitLogs)
                .WithOptional(e => e.Doctors)
                .HasForeignKey(e => e.DoctorId);

            modelBuilder.Entity<Pacients>()
                .HasMany(e => e.Documents)
                .WithOptional(e => e.Pacients)
                .HasForeignKey(e => e.PacientId);

            modelBuilder.Entity<Pacients>()
                .HasMany(e => e.VisitLogs)
                .WithOptional(e => e.Pacients)
                .HasForeignKey(e => e.PacientId);
        }
    }
}

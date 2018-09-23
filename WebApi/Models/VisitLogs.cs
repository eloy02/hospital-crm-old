using System;

namespace WebApi.Models
{
    public partial class VisitLogs
    {
        public int Id { get; set; }
        public DateTime VisitDateTime { get; set; }
        public int? PacientId { get; set; }
        public int? DoctorId { get; set; }

        public Doctors Doctor { get; set; }
        public Pacients Pacient { get; set; }
    }
}
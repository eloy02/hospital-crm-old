namespace DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VisitLogs
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime VisitDateTime { get; set; }

        public int? PacientId { get; set; }

        public int? DoctorId { get; set; }

        public virtual Doctors Doctors { get; set; }

        public virtual Pacients Pacients { get; set; }
    }
}

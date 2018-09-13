namespace DB.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Documents
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] Document { get; set; }

        public int? PacientId { get; set; }

        public virtual Pacients Pacients { get; set; }
    }
}

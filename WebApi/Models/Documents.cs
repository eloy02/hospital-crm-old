using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class Documents
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] Document { get; set; }
        public int? PacientId { get; set; }

        public Pacients Pacient { get; set; }
    }
}

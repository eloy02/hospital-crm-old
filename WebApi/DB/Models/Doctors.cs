using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class Doctors
    {
        public Doctors()
        {
            VisitLogs = new HashSet<VisitLogs>();
        }

        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PatronymicName { get; set; }
        public string Position { get; set; }
        public bool IsActive { get; set; }

        public ICollection<VisitLogs> VisitLogs { get; set; }
    }
}

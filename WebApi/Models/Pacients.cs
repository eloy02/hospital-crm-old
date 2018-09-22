using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class Pacients
    {
        public Pacients()
        {
            Documents = new HashSet<Documents>();
            VisitLogs = new HashSet<VisitLogs>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatronymicName { get; set; }
        public int PacientType { get; set; }
        public string Sity { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }
        public string PacientPhoneNumber { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public string ParentPatronymicName { get; set; }
        public string ParentPhoneNumber { get; set; }

        public ICollection<Documents> Documents { get; set; }
        public ICollection<VisitLogs> VisitLogs { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DB.Model
{
    public class Pacient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PatronymicName { get; set; }
        public int PacientType { get; set; }

        [Required]
        public string Sity { get; set; }

        [Required]
        public string Street { get; set; }

        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }
        public string PacientPhoneNumber { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public string ParentPatronymicName { get; set; }

        public Document Document { get; set; }

        public virtual ICollection<VisitLog> VisitLogs { get; set; }
    }
}
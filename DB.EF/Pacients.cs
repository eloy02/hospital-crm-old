namespace DB.EF
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Pacients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pacients()
        {
            Documents = new HashSet<Documents>();
            VisitLogs = new HashSet<VisitLogs>();
        }

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

        public string ParentPhoneNumber { get; set; }

        public virtual ICollection<Documents> Documents { get; set; }

        public virtual ICollection<VisitLogs> VisitLogs { get; set; }
    }
}
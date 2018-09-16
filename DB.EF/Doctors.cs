namespace DB.EF
{
    using System.Collections.Generic;

    public partial class Doctors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Doctors()
        {
            VisitLogs = new HashSet<VisitLogs>();
        }

        public int Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string PatronymicName { get; set; }

        public string Position { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VisitLogs> VisitLogs { get; set; }
    }
}
using Core.Types.Enumerations;

namespace Core.Types
{
    public class PacientCore
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatronymicName { get; set; }
        public EPatientType PacientType { get; set; }
        public string Sity { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }
        public string PacientPhoneNumber { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public string ParentPatronymicName { get; set; }
        public string DocumentPath { get; set; }
    }
}
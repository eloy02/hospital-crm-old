using Core.Types.BaseTypes;
using Core.Types.Enumerations;

namespace Core.Types
{
    public class Pacient : PersonBase
    {
        public int Id { get; set; }
        public EPatientType PacientType { get; set; }
        public string Sity { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }
        public string PacientPhoneNumber { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public string ParentPatronymicName { get; set; }
        public string ParentsPhoneNumber { get; set; }
        public string DocumentPath { get; set; }

        public Document Document { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {PatronymicName}";
        }
    }
}
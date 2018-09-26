using Core.Types;
using Core.Types.Enumerations;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;

namespace WebApi
{
    public static class Mapper
    {
        internal static Pacients Assign(this Pacients a, Pacient src)
        {
            a.BuildingNumber = src.BuildingNumber;
            a.FirstName = src.FirstName;
            a.FlatNumber = src.FlatNumber;
            a.LastName = src.LastName;
            a.PacientPhoneNumber = src.PacientPhoneNumber;
            a.PacientType = (int)src.PacientType;
            a.ParentFirstName = src.ParentFirstName;
            a.ParentLastName = src.ParentLastName;
            a.ParentPatronymicName = src.ParentPatronymicName;
            a.PatronymicName = src.PatronymicName;
            a.Sity = src.Sity;
            a.Street = src.Street;
            a.FlatNumber = src.FlatNumber;
            a.ParentPhoneNumber = src.ParentsPhoneNumber;
            a.Id = src.Id;

            if (src.Document != null)
                a.Documents = new List<Documents>() { new Documents().Assign(src.Document) };

            return a;
        }

        internal static Pacient Assign(this Pacient a, Pacients src)
        {
            a.BuildingNumber = src.BuildingNumber;
            a.FirstName = src.FirstName;
            a.FlatNumber = src.FlatNumber;
            a.Id = src.Id;
            a.LastName = src.LastName;
            a.PacientPhoneNumber = src.PacientPhoneNumber;
            a.PacientType = (EPatientType)src.PacientType;
            a.ParentFirstName = src.ParentFirstName;
            a.ParentLastName = src.ParentLastName;
            a.ParentPatronymicName = src.ParentPatronymicName;
            a.PatronymicName = src.PatronymicName;
            a.Sity = src.Sity;
            a.Street = src.Street;
            a.ParentsPhoneNumber = src.ParentPhoneNumber;

            return a;
        }

        internal static Doctor Assign(this Doctor a, Doctors src)
        {
            a.FirstName = src.FirstName;
            a.Id = src.Id;
            a.LastName = src.LastName;
            a.PatronymicName = src.PatronymicName;
            a.Position = src.Position;
            a.DisplayName = $"{a.LastName} {a.FirstName} {a.PatronymicName} - {a.Position}";

            return a;
        }

        internal static Doctors Assign(this Doctors a, Doctor src)
        {
            a.FirstName = src.FirstName;
            a.LastName = src.LastName;
            a.PatronymicName = src.PatronymicName;
            a.Position = src.Position;
            a.Id = src.Id;

            return a;
        }

        internal static Document Assign(this Document a, Documents src)
        {
            a.Id = src.Id;
            a.Content = src.Document.ToList();
            a.Name = src.FileName;

            return a;
        }

        internal static Documents Assign(this Documents a, Document src)
        {
            a.Document = src.Content.ToArray();
            a.FileName = src.Name;

            return a;
        }

        internal static VisitLog Assign(this VisitLog a, VisitLogs src)
        {
            a.Id = src.Id;
            a.Pacient = new Pacient().Assign(src.Pacient);
            a.Doctor = new Doctor().Assign(src.Doctor);
            a.VisitDateTime = src.VisitDateTime;

            return a;
        }

        internal static VisitLogs Assign(this VisitLogs a, VisitLog src)
        {
            a.Id = src.Id;
            a.PacientId = src.Pacient.Id;
            a.VisitDateTime = src.VisitDateTime;
            a.DoctorId = src.Doctor.Id;

            return a;
        }
    }
}
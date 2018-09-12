using Core.Types;
using DB.Model;

namespace Core
{
    public static class Mapper
    {
        internal static Pacient Assign(this Pacient a, PacientCore src)
        {
            a.BuildingNumber = src.BuildingNumber;
            a.Document = new Document
            {
                FilePath = src.DocumentPath
            };
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

            return a;
        }
    }
}
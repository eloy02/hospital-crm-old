using Core.Types;
using DB.EF;

namespace Core
{
    public static class Mapper
    {
        internal static Pacients Assign(this Pacients a, PacientCore src)
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
            return a;
        }
    }
}
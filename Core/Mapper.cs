﻿using Core.Types;
using Core.Types.Enumerations;
using DB.EF;

namespace Core
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
    }
}
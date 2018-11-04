using Core.Types;
using ExcelReports.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExcelReports
{
    public class ExcelReports : IExcelReports
    {
        private class PacientVisits
        {
            public DateTime VisitDate { get; set; }
            public string Doctor { get; set; }
            public string Role { get; set; }
        }

        private class DoctorsVisits
        {
            public DateTime VisitDate { get; set; }
            public string PacientFio { get; set; }
            public string Address { get; set; }
            public string PacientPhoneNumber { get; set; }
            public string ParentsFio { get; set; }
            public string ParentsPhoneNumber { get; set; }
        }

        public string CreateVisitLogReport(IEnumerable<VisitLog> visits, Pacient pacient)
        {
            var pacientFio = $"{pacient.LastName} {pacient.FirstName} {pacient.PatronymicName}";
            var pacientAddress = $"{pacient.Sity} {pacient.Street} {pacient.BuildingNumber} {pacient.FlatNumber}";
            var filename = @".\CompletedReports" + $"\\История_посещений_пациента_{pacientFio}.xlsx";

            var items = visits.Select(v => new PacientVisits
            {
                Doctor = $"{v.Doctor.LastName} {v.Doctor.FirstName} {v.Doctor.PatronymicName}",
                Role = v.Doctor.Position,
                VisitDate = v.VisitDateTime
            }).ToList();

            var template = Path.Combine(@".\ExcelTemplates", "PacientVisitHistory.xlsx").ToString();
            var fi = new FileInfo(template);

            using (var p = new ExcelPackage(fi))
            {
                var ws = p.Workbook.Worksheets[1];

                var r = ws.Cells[4, 3];
                if (r != null)
                {
                    r.Value = pacientFio;

                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                r = ws.Cells[5, 3];
                if (r != null)
                {
                    r.Value = pacientAddress;

                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                r = ws.Cells[9, 2];
                r.LoadFromCollection(items);
                if (r != null)
                {
                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                if (Directory.Exists(@".\CompletedReports"))
                {
                    p.SaveAs(new FileInfo(filename));
                }
                else
                {
                    Directory.CreateDirectory(@".\CompletedReports");
                    p.SaveAs(new FileInfo(filename));
                }
            }

            return filename;
        }

        public string CreateVisitLogReport(IEnumerable<VisitLog> visits, Doctor doctor)
        {
            var doctorFio = $"{doctor.LastName} {doctor.FirstName} {doctor.PatronymicName}";
            var filename = @".\CompletedReports" + $"\\История_посещений_доктора_{doctorFio}.xlsx";
            var template = Path.Combine(@".\ExcelTemplates", "DoctorVisitsHistory.xlsx").ToString();
            var fi = new FileInfo(template);

            var items = visits.Select(v => new DoctorsVisits
            {
                Address = $"{v.Pacient.Sity} {v.Pacient.Street} {v.Pacient.BuildingNumber} {v.Pacient.FlatNumber}",
                PacientFio = $"{v.Pacient.LastName} {v.Pacient.FirstName} {v.Pacient.LastName}",
                PacientPhoneNumber = $"{v.Pacient.PacientPhoneNumber}",
                ParentsFio = $"{v.Pacient.ParentLastName} {v.Pacient.ParentFirstName} {v.Pacient.ParentPatronymicName}",
                ParentsPhoneNumber = $"{v.Pacient.ParentsPhoneNumber}",
                VisitDate = v.VisitDateTime
            }).ToList();

            using (var p = new ExcelPackage(fi))
            {
                var ws = p.Workbook.Worksheets[1];

                var r = ws.Cells[4, 3];
                if (r != null)
                {
                    r.Value = doctorFio;

                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                r = ws.Cells[5, 3];
                if (r != null)
                {
                    r.Value = doctor.Position;

                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                r = ws.Cells[9, 2];
                r.LoadFromCollection(items);
                if (r != null)
                {
                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                if (Directory.Exists(@".\CompletedReports"))
                {
                    p.SaveAs(new FileInfo(filename));
                }
                else
                {
                    Directory.CreateDirectory(@".\CompletedReports");
                    p.SaveAs(new FileInfo(filename));
                }

                return filename;
            }
        }
    }
}
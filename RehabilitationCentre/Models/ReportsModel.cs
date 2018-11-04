using Core.Types;
using ExcelReports.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Interfaces;

namespace RehabilitationCentre.Models
{
    public class ReportsModel
    {
        private readonly IWebClient webClient;
        private readonly IExcelReports excelReports;

        public List<Pacient> PacientsList = new List<Pacient>();
        public List<Doctor> Doctors = new List<Doctor>();

        public ReportsModel(IWebClient webClient, IExcelReports excelReports)
        {
            this.webClient = webClient;
            this.excelReports = excelReports;
        }

        public async Task<IList<Pacient>> GetPacientsAsync()
        {
            var r = await webClient.GetPacientsAsync();
            if (r != null)
            {
                PacientsList.Clear();
                PacientsList.AddRange(r);

                return r.ToList();
            }
            else return null;
        }

        public async Task<IList<Doctor>> GetDoctorsAsync()
        {
            var r = await webClient.GetDoctorsAsync();

            if (r != null)
            {
                Doctors.Clear();
                Doctors.AddRange(r);

                return r.ToList();
            }
            else return null;
        }

        public async Task CreateVisitReportAsync(Pacient pacient)
        {
            var visits = await webClient.GetVisitLogAsync(pacient);

            if (visits != null)
            {
                await Task.Run(() =>
                {
                    var file = excelReports.CreateVisitLogReport(visits, pacient);

                    if (File.Exists(file))
                    {
                        var excel = System.Diagnostics.Process.Start(file);
                    }
                });
            }
        }

        public async Task CreateVisitReportAsync(Doctor doctor)
        {
            var visits = await webClient.GetVisitLogAsync(doctor);

            if (visits != null)
            {
                await Task.Run(() =>
                {
                    var file = excelReports.CreateVisitLogReport(visits, doctor);

                    if (File.Exists(file))
                    {
                        var excel = System.Diagnostics.Process.Start(file);
                    }
                });
            }
        }
    }
}
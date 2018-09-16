using Castle.Windsor;
using Core.Interfaces;
using Core.Types;
using DB.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class Core : ICore
    {
        private static IWindsorContainer _container;
        private DbServise DB = new DbServise();

        public static void Init(IWindsorContainer ioc)
        {
            _container = ioc;
            //Log = _container.Resolve<ILogger>();
        }

        public string GetProgrammTempFolder()
        {
            return Directory.GetCurrentDirectory() + @"\Temp";
        }

        public async Task SavePacientAsync(Pacient pacient)
        {
            byte[] file = null;
            Pacients pacientDb = null;

            var t1 = Task.Run(() =>
            {
                using (var stream = new FileStream(pacient.DocumentPath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        file = reader.ReadBytes((int)stream.Length);
                    }
                }
            });

            var t2 = Task.Run(() => pacientDb = new Pacients().Assign(pacient));

            await Task.WhenAll(t1, t2);

            pacientDb.Documents = new List<Documents>()
            {
                new Documents {Document = file, FileName = $"{pacientDb.LastName}{pacientDb.FirstName}" }
            };

            await DB.SavePacientAsync(pacientDb);
        }

        public async Task<IEnumerable<Pacient>> GetAllPacientsAsync()
        {
            var raw = await DB.GetPacientsAsync();

            var pacients = raw.Select(p => new Pacient().Assign(p)).ToList();

            return pacients;
        }

        public async Task ShowPdfDocumentAsync(Pacient pacient)
        {
            var path = GetProgrammTempFolder();
            Directory.CreateDirectory(path);

            var raw = await DB.GetDocumentByPacientAsync(new Pacients().Assign(pacient));

            var file = $"{path}\\{Guid.NewGuid().ToString()}.pdf";
            File.WriteAllBytes(file, raw);

            var pdfProc = System.Diagnostics.Process.Start(file);
        }

        private void pdfProcess_Exited(object sender, EventArgs e)
        {
            File.Delete("F:\\hello.pdf");
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
        {
            var raw = await DB.GetDoctorsAsync();

            return raw.Select(r => new Doctor().Assign(r)).ToList();
        }

        public async Task SetPacientVisit(Pacient pacient, Doctor doctor)
        {
            var visit = new VisitLogs
            {
                DoctorId = doctor.Id,
                PacientId = pacient.Id,
                VisitDateTime = DateTime.Now
            };

            await DB.SetPacientVisitAsync(visit);
        }
    }
}
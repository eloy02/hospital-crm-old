using Castle.Windsor;
using Core.Interfaces;
using Core.Types;
using DB.EF;
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
            var raw = await DB.GetDocumentByPacientAsync(new Pacients().Assign(pacient));

            File.WriteAllBytes("F:\\hello.pdf", raw);
        }
    }
}
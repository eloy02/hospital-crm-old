using Core.Types;
using System.IO;
using System.Threading.Tasks;
using WebClient.Interfaces;

namespace PacientRegistry.Models
{
    public class PacientInfoModel
    {
        private IWebClient WebClientApi;

        public PacientInfoModel(IWebClient webclient)
        {
            WebClientApi = webclient;
        }

        public async Task OpenPacientDocument(Pacient pacient)
        {
            var path = Directory.GetCurrentDirectory() + @"\Temp";
            var file = $"{path}\\{pacient.LastName}{pacient.FirstName}{pacient.PatronymicName}.pdf";

            if (File.Exists(file))
            {
                var pdfProc = System.Diagnostics.Process.Start(file);
            }
            else
            {
                var doc = await WebClientApi.GetPacientDocumentAsync(pacient);
                Directory.CreateDirectory(path);

                File.WriteAllBytes(file, doc.Content.ToArray());
                var pdfProc = System.Diagnostics.Process.Start(file);
                doc = null;
            }
        }

        public async Task<bool> UpdatePacientAsync(Pacient pacient)
        {
            var res1 = false;
            var res2 = false;

            var t1 = Task.Run(async () =>
            {
                if (!string.IsNullOrEmpty(pacient.DocumentPath))
                {
                    var r = await WebClientApi.UpdatePacientDocumentAsync(pacient.DocumentPath, pacient);
                    res1 = r;
                }
                else res1 = true;
            });

            res2 = await WebClientApi.UpdatePacientsDataAsync(pacient);

            if (res1 && res2)
                return true;
            else return false;
        }
    }
}
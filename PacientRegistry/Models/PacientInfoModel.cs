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

        public async Task UpdatePacientAsync(Pacient pacient)
        {
            await WebClientApi.UpdatePacientsDataAsync(pacient);
        }
    }
}
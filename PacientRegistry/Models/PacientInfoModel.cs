using Core.Types;
using System;
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
            var doc = await WebClientApi.GetPacientDocumentAsync(pacient);

            var path = Directory.GetCurrentDirectory() + @"\Temp";
            Directory.CreateDirectory(path);
            var file = $"{path}\\{Guid.NewGuid().ToString()}.pdf";
            File.WriteAllBytes(file, doc.Content.ToArray());
            var pdfProc = System.Diagnostics.Process.Start(file);
        }

        public async Task UpdatePacientAsync(Pacient pacient)
        {
            await WebClientApi.UpdatePacientsDataAsync(pacient);
        }
    }
}
using Core.Types;
using KladrApiClient;
using PacientRegistry.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using WebClient;

namespace PacientRegistry.Models
{
    public class PacientInfoModel
    {
        private const string RegionId = "0200000000000";
        private WebClientApi WebClientApi = new WebClientApi();
        private Guid WebToken = new Guid();
        private KladrClient kladrClient;
        private PacientInfoViewModel ViewModel;

        public PacientInfoModel(PacientInfoViewModel viewModel, Guid webToken)
        {
            WebToken = webToken;
            kladrClient = new KladrClient("some_token", "some_key");
            ViewModel = viewModel;
        }

        public async Task OpenPacientDocument(Pacient pacient)
        {
            var doc = await WebClientApi.GetPacientDocumentAsync(WebToken, pacient);

            var path = Directory.GetCurrentDirectory() + @"\Temp";
            Directory.CreateDirectory(path);
            var file = $"{path}\\{Guid.NewGuid().ToString()}.pdf";
            File.WriteAllBytes(file, doc.Content.ToArray());
            var pdfProc = System.Diagnostics.Process.Start(file);
        }

        public async Task UpdatePacientAsync(Pacient pacient)
        {
            await WebClientApi.UpdatePacientsDataAsync(WebToken, pacient);
        }
    }
}
using Core.Types;
using KladrApiClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebClient;

namespace PacientRegistry.Models
{
    public class ShellModel
    {
        private const string RegionId = "0200000000000";

        private WebClientApi WebClientApi = new WebClientApi();
        public Guid WebToken;

        private KladrClient kladrClient;
        private ShellViewModel ViewModel;

        public List<Pacient> Pacients = new List<Pacient>();

        public ShellModel(ShellViewModel viewModel)
        {
            kladrClient = new KladrClient("some_token", "some_key");
            ViewModel = viewModel;
        }

        public void LoadSities()
        {
            kladrClient.FindAddress(new Dictionary<string, string>
                                        {
                                            {"contentType", "city"},
                                            {"regionId", RegionId},
                                        }, ViewModel.SetKladrSities);
        }

        public void SearchSity(string text)
        {
            kladrClient.FindAddress(new Dictionary<string, string>
                                        {
                                            {"query", text },
                                            {"contentType", "city"},
                                            {"regionId", RegionId},
                                        }, ViewModel.SetKladrSities);
        }

        public void LoadStreetsForSity(string cityId)
        {
            kladrClient.FindAddress(new Dictionary<string, string>
                                        {
                                            {"contentType", "street"},
                                            {"cityId", cityId},
                                        }, ViewModel.SetKladrStreets);
        }

        public void SearchStreets(string cityId, string text)
        {
            kladrClient.FindAddress(new Dictionary<string, string>
                                        {
                                            {"query", text },
                                            {"contentType", "street"},
                                            {"cityId", cityId},
                                        }, ViewModel.SetKladrStreets);
        }

        public void LoadBuildingsForStreet(string streetId)
        {
            kladrClient.FindAddress(new Dictionary<string, string>
                                        {
                                            {"contentType", "building"},
                                            {"streetId", streetId},
                                        }, ViewModel.SetKladrBuildings);
        }

        public async Task SavePacientAsync()
        {
            var pacient = new Pacient()
            {
                BuildingNumber = ViewModel.BuildingNumber,
                DocumentPath = ViewModel.PdfPath,
                FirstName = ViewModel.PacientFirstName,
                FlatNumber = ViewModel.FlatNumber,
                LastName = ViewModel.PacientLastName,
                PacientPhoneNumber = ViewModel.PacientPhoneNumber,
                PacientType = ViewModel.SelectedPacientType.Value,
                ParentFirstName = ViewModel.ParentFirstName,
                ParentLastName = ViewModel.ParentLastName,
                ParentPatronymicName = ViewModel.ParentPatronymicName,
                PatronymicName = ViewModel.PacientPatronymicName,
                ParentsPhoneNumber = ViewModel.ParentPhoneNumber,
                Sity = ViewModel.Sity,
                Street = ViewModel.Street
            };

            byte[] file = null;

            using (var stream = new FileStream(pacient.DocumentPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }

            var doc = new Document
            {
                Content = file.ToList(),
                Name = $"{pacient.LastName}{pacient.FirstName}{pacient.PatronymicName}"
            };

            pacient.Document = doc;

            await WebClientApi.SavePacientAsync(WebToken, pacient);
        }

        public async Task<Guid> GetProgrammTokenAsync()
        {
            return await WebClientApi.GetProgrammTokenAsync();
        }

        public async Task<List<Pacient>> GetPacientsAsync()
        {
            try
            {
                var p = await WebClientApi.GetPacientsAsync(WebToken);

                if (p != null)
                {
                    Pacients.Clear();
                    Pacients.AddRange(p);

                    return p.ToList();
                }
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteToken()
        {
            await WebClientApi.DeleteToken(WebToken);
        }

        //public async Task UpdatePacientAsync(Pacient pacient)
        //{
        //    await WebClientApi.UpdatePacientData(pacient);
        //}
    }
}
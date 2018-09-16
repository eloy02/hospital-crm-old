using Core.Interfaces;
using Core.Types;
using KladrApiClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PacientRegistry.Models
{
    public class ShellModel
    {
        private const string RegionId = "0200000000000";

        private ICore Core = new Core.Core();

        private KladrClient kladrClient;
        private ShellViewModel ViewModel;

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

            await Core.SavePacientAsync(pacient);
        }
    }
}
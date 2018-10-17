using Castle.Windsor;
using Core.Types;
using RestSharp;
using RestSharp.Serializers.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Interfaces;

using JsonRest = RestSharp.Serializers.Newtonsoft.Json;

namespace WebClient
{
    public class WebClientApi : IWebClient
    {
        private static IWindsorContainer _container;

        //private Uri BaseUrl = new Uri("https://localhost:44391/api");

        private Uri BaseUrl = new Uri("http://eloy102-001-site1.dtempurl.com/api");
        private string ProgrammGuid = "2F5F714611C34EC5A2D6F06DEFD0AB084A940477EBED4BE4BC62-452D6B92D972";

        private Guid Token;
        private User CurrentUser;
        private string UserPassword;

        internal static void Init(IWindsorContainer ioc)
        {
            _container = ioc;
        }

        private async Task<T> ExecuteAsync<T>(JsonRest.RestRequest request) where T : new()
        {
            var client = new RestClient(BaseUrl);

            request.RequestFormat = DataFormat.Json;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            request.AddParameter("token", Token, ParameterType.QueryString);

            IRestResponse<T> response = await client.ExecuteTaskAsync<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await GetProgrammTokenAsync();

                await ExecuteAsync<T>(request);
            }

            if (!response.IsSuccessful)
                return default(T);

            return response.Data;
        }

        private async Task<IRestResponse> ExecuteAsync(JsonRest.RestRequest request)
        {
            var client = new RestClient(BaseUrl);

            request.RequestFormat = DataFormat.Json;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            request.AddParameter("token", Token, ParameterType.QueryString);

            IRestResponse response = await client.ExecuteTaskAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await GetProgrammTokenAsync();

                await ExecuteAsync(request);
            }

            return response;
        }

        public async Task<bool> GetProgrammTokenAsync(User user = null, string password = null)
        {
            var client = new RestClient(BaseUrl);

            var request = new RestSharp.RestRequest(Method.GET)
            {
                Resource = "authenticator/login"
            };

            if (CurrentUser == null && string.IsNullOrEmpty(UserPassword))
            {
                request.AddParameter("programmGuid", ProgrammGuid, ParameterType.QueryString);
                request.AddParameter("userId", user.Id, ParameterType.QueryString);
                request.AddParameter("password", password, ParameterType.QueryString);
            }
            else
            {
                request.AddParameter("programmGuid", ProgrammGuid, ParameterType.QueryString);
                request.AddParameter("userId", CurrentUser.Id, ParameterType.QueryString);
                request.AddParameter("password", UserPassword, ParameterType.QueryString);
            }

            var r = await client.ExecuteTaskAsync<Guid>(request);

            if (r.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Ошибка авторизации программы");
            else if (!r.IsSuccessful)
            {
                return false;
            }
            else
            {
                Token = r.Data;

                if (user != null)
                    CurrentUser = user;

                if (password != null)
                    UserPassword = password;

                return true;
            }
        }

        public async Task<IEnumerable<Pacient>> GetPacientsAsync()
        {
            var request = new JsonRest.RestRequest(Method.GET)
            {
                Resource = "pacients"
            };

            var r = await ExecuteAsync<List<Pacient>>(request);

            return r;
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
        {
            var request = new JsonRest.RestRequest(Method.GET)
            {
                Resource = "CommonData/doctors"
            };

            var r = await ExecuteAsync<List<Doctor>>(request);

            return r;
        }

        public async Task<bool> SavePacientAsync(Pacient pacient)
        {
            var request = new JsonRest.RestRequest(Method.POST)
            {
                Resource = "pacients"
            };

            request.AddJsonBody(pacient);

            var r = await ExecuteAsync(request);

            if (r.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else return false;
        }

        public async Task<bool> SavePacientsDocumentAsync(int pacientId, Document doc)
        {
            var request = new JsonRest.RestRequest(Method.POST)
            {
                Resource = "Documents"
            };

            request.AddParameter("pacientId", pacientId, ParameterType.QueryString);
            request.AddJsonBody(doc);

            var r = await ExecuteAsync(request);

            if (r.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else return false;
        }

        public async Task<Document> GetPacientDocumentAsync(Pacient pacient)
        {
            var request = new JsonRest.RestRequest(Method.GET)
            {
                Resource = "Documents"
            };

            request.AddParameter("pacientId", pacient.Id, ParameterType.QueryString);

            var r = await ExecuteAsync<Document>(request);

            return r;
        }

        public async Task<bool> SavePacientVisitAsync(Pacient pacient, Doctor doc, DateTime visitDateTime)
        {
            var visit = new VisitLog
            {
                Doctor = doc,
                Pacient = pacient,
                VisitDateTime = visitDateTime
            };

            var request = new JsonRest.RestRequest(Method.POST)
            {
                Resource = "PacientVisits"
            };

            request.JsonSerializer = new NewtonsoftJsonSerializer();

            request.AddJsonBody(visit);

            var r = await ExecuteAsync(request);

            if (r.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else return false;
        }

        public async Task<bool> UpdatePacientDocumentAsync(string filePath, Pacient pacient)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                byte[] file = null;

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        file = reader.ReadBytes((int)stream.Length);
                    }
                }

                var doc = new Document
                {
                    Content = file.ToList(),
                    Id = 0,
                    Name = $"{pacient.LastName}{pacient.FirstName}"
                };

                var request = new JsonRest.RestRequest(Method.PUT)
                {
                    Resource = "Documents"
                };

                request.AddParameter("pacientId", pacient.Id, ParameterType.QueryString);
                request.AddJsonBody(doc);

                var r = await ExecuteAsync(request);

                if (r.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else return false;
            }
            else return false;
        }

        public async Task<bool> UpdatePacientsDataAsync(Pacient pacient)
        {
            var request = new JsonRest.RestRequest(Method.PUT)
            {
                Resource = "Pacients"
            };

            request.AddParameter("pacientId", pacient.Id, ParameterType.QueryString);
            request.AddJsonBody(pacient);

            var r = await ExecuteAsync(request);

            if (r.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else return false;
        }

        public async Task DeleteToken()
        {
            var client = new RestClient(BaseUrl);

            var request = new JsonRest.RestRequest(Method.DELETE)
            {
                Resource = "Authenticator"
            };

            request.AddParameter("token", Token, ParameterType.QueryString);

            var r = await client.ExecuteTaskAsync(request);

            if (!r.IsSuccessful)
                throw new Exception($"Request status = {r.ResponseStatus}");
        }

        public async Task<IEnumerable<VisitLog>> GetVisitLogsForPacientAsync(Pacient pacient)
        {
            var request = new JsonRest.RestRequest(Method.GET)
            {
                Resource = "PacientVisits"
            };

            request.JsonSerializer = new NewtonsoftJsonSerializer();

            request.AddParameter("pacientId", pacient.Id, ParameterType.QueryString);

            var r = await ExecuteAsync<List<VisitLog>>(request);

            r.ForEach(d => d.VisitDateTime = d.VisitDateTime.AddHours(3));

            return r;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var client = new RestClient(BaseUrl);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            var request = new JsonRest.RestRequest(Method.GET)
            {
                Resource = "CommonData/users"
            };

            request.AddParameter("programmGuid", ProgrammGuid, ParameterType.QueryString);

            request.JsonSerializer = new NewtonsoftJsonSerializer();

            var r = await ExecuteAsync<List<User>>(request);

            return r;
        }
    }
}
using Core.Types;
using Core.Types.DTO;
using Core.Types.Enumerations;
using RestSharp;
using RestSharp.Serializers.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using WebClient.Interfaces;

using JsonRest = RestSharp.Serializers.Newtonsoft.Json;

namespace WebClient
{
    public class WebClientApi : IWebClient
    {
        //private Uri BaseUrl = new Uri("https://localhost:44391/api");
        private static string TempPath = Directory.GetCurrentDirectory() + @"\Temp";

        private Uri BaseUrl = new Uri("http://eloy102-001-site1.dtempurl.com/api");
        private string ProgrammGuid = "2F5F714611C34EC5A2D6F06DEFD0AB084A940477EBED4BE4BC62-452D6B92D972";

        private Guid Token;
        private User CurrentUser;
        private string UserPassword;

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

            if (!response.IsSuccessful && response.ResponseStatus != ResponseStatus.Completed)
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

        public async Task<(bool requestResult, EAccessGroup? accessGroup)> GetProgrammTokenAsync(User user = null, string password = null)
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

            var r = await client.ExecuteTaskAsync<AuthResult>(request);

            if (r.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Ошибка авторизации программы");
            else if (!r.IsSuccessful)
            {
                return (false, null);
            }
            else
            {
                Token = r.Data.SessionGuid;

                if (user != null)
                    CurrentUser = user;

                if (password != null)
                    UserPassword = password;

                return (true, r.Data.UserAccess);
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

        public async Task<string> GetPacientDocumentAsync(Pacient pacient)
        {
            var uriBuilder = new UriBuilder($"{BaseUrl}/Documents");
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["token"] = Token.ToString();
            parameters["pacientId"] = pacient.Id.ToString();
            uriBuilder.Query = parameters.ToString();

            string filename = "";
            var uri = uriBuilder.Uri;
            var request = (HttpWebRequest)WebRequest.Create(uri);

            request.Method = "GET";

            if (!Directory.Exists(TempPath))
            {
                Directory.CreateDirectory(TempPath);
            }

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                string path = response.Headers["Content-Disposition"];
                if (string.IsNullOrWhiteSpace(path))
                {
                    filename = Path.GetFileName(uri.LocalPath);
                }
                else
                {
                    ContentDisposition contentDisposition = new ContentDisposition(path);
                    filename = contentDisposition.FileName;
                }

                var responseStream = response.GetResponseStream();
                using (var fileStream = File.Create(System.IO.Path.Combine(TempPath, filename)))
                {
                    responseStream.CopyTo(fileStream);
                }
            }

            return Path.Combine(TempPath, filename);
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
                Resource = "VisitLogs"
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

            if (!r.IsSuccessful && r.ResponseStatus != ResponseStatus.Completed)
                throw new Exception($"Request status = {r.ResponseStatus}");
        }

        public async Task<IEnumerable<VisitLog>> GetVisitLogAsync(Pacient pacient)
        {
            var request = new JsonRest.RestRequest(Method.GET)
            {
                Resource = "VisitLogs/pacientsvisitlogs"
            };

            request.JsonSerializer = new NewtonsoftJsonSerializer();

            request.AddParameter("pacientId", pacient.Id, ParameterType.QueryString);

            var r = await ExecuteAsync<List<VisitLog>>(request);

            if (r != null)
                r.ForEach(d => d.VisitDateTime = d.VisitDateTime.AddHours(3));

            return r;
        }

        public async Task<IEnumerable<VisitLog>> GetVisitLogAsync(Doctor doctor)
        {
            var request = new JsonRest.RestRequest(Method.GET)
            {
                Resource = "VisitLogs/doctorsvisitlogs"
            };

            request.JsonSerializer = new NewtonsoftJsonSerializer();

            request.AddParameter("doctorId", doctor.Id, ParameterType.QueryString);

            var r = await ExecuteAsync<List<VisitLog>>(request);

            if (r != null)
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

        public async Task<User> AddUserAsync(User user, string password)
        {
            var request = new JsonRest.RestRequest(Method.POST)
            {
                Resource = "admin/users"
            };

            request.AddParameter("password", password, ParameterType.QueryString);
            request.AddJsonBody(user);

            return await ExecuteAsync<User>(request);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var request = new JsonRest.RestRequest(Method.PUT)
            {
                Resource = "admin/users"
            };

            request.AddJsonBody(user);

            return await ExecuteAsync<User>(request);
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            var request = new JsonRest.RestRequest(Method.DELETE)
            {
                Resource = "admin/users"
            };

            request.AddJsonBody(user);

            var r = await ExecuteAsync(request);

            if (r.StatusCode == HttpStatusCode.OK)
                return true;
            else return false;
        }

        public async Task<Doctor> AddDoctorAsync(Doctor doctor)
        {
            var request = new JsonRest.RestRequest(Method.POST)
            {
                Resource = "admin/doctors"
            };

            request.AddJsonBody(doctor);

            return await ExecuteAsync<Doctor>(request);
        }

        public async Task<Doctor> UpdateDoctorAsync(Doctor doctor)
        {
            var request = new JsonRest.RestRequest(Method.PUT)
            {
                Resource = "admin/doctors"
            };

            request.AddJsonBody(doctor);

            return await ExecuteAsync<Doctor>(request);
        }

        public async Task<bool> DeleteDoctorAsync(Doctor doctor)
        {
            var request = new JsonRest.RestRequest(Method.DELETE)
            {
                Resource = "admin/doctors"
            };

            request.AddJsonBody(doctor);

            var r = await ExecuteAsync(request);

            if (r.StatusCode == HttpStatusCode.OK)
                return true;
            else return false;
        }

        public async Task<bool> DeletePacientAsync(Pacient pacient)
        {
            var request = new JsonRest.RestRequest(Method.DELETE)
            {
                Resource = "pacients"
            };

            request.AddParameter("pacientId", pacient.Id, ParameterType.QueryString);

            var r = await ExecuteAsync(request);

            if (r.StatusCode == HttpStatusCode.OK)
                return true;
            else return false;
        }
    }
}
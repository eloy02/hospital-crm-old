﻿using Castle.Windsor;
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

        internal static void Init(IWindsorContainer ioc)
        {
            _container = ioc;
        }

        private async Task<T> ExecuteAsync<T>(JsonRest.RestRequest request) where T : new()
        {
            var client = new RestClient(BaseUrl);

            request.RequestFormat = DataFormat.Json;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            IRestResponse<T> response = await client.ExecuteTaskAsync<T>(request);

            if (!response.IsSuccessful)
                return default(T);

            return response.Data;
        }

        public async Task GetProgrammTokenAsync()
        {
            var request = new JsonRest.RestRequest(Method.GET)
            {
                Resource = "authenticator"
            };

            request.AddParameter("programmGuid", ProgrammGuid, ParameterType.QueryString);

            var r = await ExecuteAsync<Guid>(request);

            if (r == default(Guid))
                throw new Exception("Ошибка авторизации программы");

            Token = r;
        }

        public async Task<IEnumerable<Pacient>> GetPacientsAsync()
        {
            var request = new JsonRest.RestRequest(Method.GET)
            {
                Resource = "pacients"
            };

            request.AddParameter("token", Token, ParameterType.QueryString);

            var r = await ExecuteAsync<List<Pacient>>(request);

            return r;
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
        {
            var request = new JsonRest.RestRequest(Method.GET)
            {
                Resource = "CommonData/doctors"
            };

            request.AddParameter("token", Token, ParameterType.QueryString);

            var r = await ExecuteAsync<List<Doctor>>(request);

            return r;
        }

        public async Task SavePacientAsync(Pacient pacient)
        {
            var client = new RestClient(BaseUrl);

            var request = new JsonRest.RestRequest(Method.POST)
            {
                Resource = "pacients"
            };

            request.AddParameter("token", Token, ParameterType.QueryString);
            request.AddJsonBody(pacient);

            var r = await client.ExecutePostTaskAsync(request);

            if (!r.IsSuccessful)
                throw new Exception($"Request status = {r.ResponseStatus}");
        }

        public async Task SavePacientsDocumentAsync(int pacientId, Document doc)
        {
            var client = new RestClient(BaseUrl);

            var request = new JsonRest.RestRequest(Method.POST)
            {
                Resource = "Documents"
            };

            request.AddParameter("token", Token, ParameterType.QueryString);
            request.AddParameter("pacientId", pacientId, ParameterType.QueryString);
            request.AddJsonBody(doc);

            var r = await client.ExecutePostTaskAsync(request);

            if (!r.IsSuccessful)
                throw new Exception($"Request status = {r.ResponseStatus}");
        }

        public async Task<Document> GetPacientDocumentAsync(Pacient pacient)
        {
            var client = new RestClient(BaseUrl);

            var request = new JsonRest.RestRequest(Method.GET)
            {
                Resource = "Documents"
            };

            request.AddParameter("token", Token, ParameterType.QueryString);

            request.AddParameter("pacientId", pacient.Id, ParameterType.QueryString);

            var r = await client.ExecuteGetTaskAsync<Document>(request);

            return r.Data;
        }

        public async Task SavePacientVisitAsync(Pacient pacient, Doctor doc)
        {
            var visit = new VisitLog
            {
                Doctor = doc,
                Pacient = pacient,
                VisitDateTime = DateTime.Now
            };

            var client = new RestClient(BaseUrl);

            var request = new JsonRest.RestRequest(Method.POST)
            {
                Resource = "PacientVisits"
            };

            request.JsonSerializer = new NewtonsoftJsonSerializer();

            request.AddParameter("token", Token, ParameterType.QueryString);
            request.AddJsonBody(visit);

            var r = await client.ExecutePostTaskAsync(request);

            if (!r.IsSuccessful)
                throw new Exception($"Request status = {r.ResponseStatus}");
        }

        public async Task UpdatePacientsDataAsync(Pacient pacient)
        {
            var t1 = Task.Run(async () =>
            {
                if (!string.IsNullOrEmpty(pacient.DocumentPath))
                {
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
                        Id = 0,
                        Name = $"{pacient.LastName}{pacient.FirstName}"
                    };

                    var client = new RestClient(BaseUrl);

                    var request = new JsonRest.RestRequest(Method.PUT)
                    {
                        Resource = "Documents"
                    };

                    request.AddParameter("token", Token, ParameterType.QueryString);
                    request.AddParameter("pacientId", pacient.Id, ParameterType.QueryString);
                    request.AddJsonBody(doc);

                    var r = await client.ExecuteTaskAsync(request);
                }
            });

            var t2 = Task.Run(async () =>
            {
                var client = new RestClient(BaseUrl);

                var request = new JsonRest.RestRequest(Method.PUT)
                {
                    Resource = "Pacients"
                };

                request.AddParameter("pacientId", pacient.Id, ParameterType.QueryString);
                request.AddJsonBody(pacient);

                var r = await client.ExecuteTaskAsync(request);
            });

            await Task.WhenAll(t1, t2);
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

            request.AddParameter("token", Token, ParameterType.QueryString);

            request.AddParameter("pacientId", pacient.Id, ParameterType.QueryString);

            var r = await ExecuteAsync<List<VisitLog>>(request);

            return r;
        }
    }
}
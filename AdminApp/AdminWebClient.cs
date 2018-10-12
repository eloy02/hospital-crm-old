using Core.Types;
using RestSharp;
using System;
using System.Threading.Tasks;
using JsonRest = RestSharp.Serializers.Newtonsoft.Json;

namespace AdminApp
{
    public static class AdminWebClient
    {
        private static Uri BaseUrl = new Uri("https://localhost:44391/api");
        //private static Uri BaseUrl = new Uri("http://eloy102-001-site1.dtempurl.com/api");

        private const string ProgrammGuid = "6085DA8AA204-45EA-A65B-AB0624EAC047-5361A0C2-7797-4F5B-A8E6-DCFA611A05E5";

        public static async Task AddUser(User user, string password)
        {
            var client = new RestClient(BaseUrl);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            var request = new JsonRest.RestRequest(Method.POST)
            {
                Resource = "CommonData/users"
            };

            request.AddParameter("programmGuid", ProgrammGuid, ParameterType.QueryString);
            request.AddJsonBody(user);
            request.AddParameter("password", password, ParameterType.QueryString);

            var r = await client.ExecuteTaskAsync(request);
        }
    }
}
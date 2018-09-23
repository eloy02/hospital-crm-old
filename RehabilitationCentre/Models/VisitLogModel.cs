using Core.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient;

namespace RehabilitationCentre.Models
{
    public class VisitLogModel
    {
        public Guid WebToken;
        private WebClientApi WebClientApi = new WebClientApi();

        public VisitLogModel()
        {
        }

        public async Task<IEnumerable<VisitLog>> GetVisistLogsForPacientAsync(Pacient pacient)
        {
            var res = await WebClientApi.GetVisitLogsForPacientAsync(WebToken, pacient);

            return res;
        }

        public async Task GetProgrammTokenAsync()
        {
            WebToken = await WebClientApi.GetProgrammTokenAsync();
        }
    }
}
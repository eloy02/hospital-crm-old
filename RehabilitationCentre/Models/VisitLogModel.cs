using Core.Types;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Interfaces;

namespace RehabilitationCentre.Models
{
    public class VisitLogModel
    {
        public readonly IWebClient WebClient;

        public VisitLogModel(IWebClient webClient)
        {
            WebClient = webClient;
        }

        public async Task<IEnumerable<VisitLog>> GetVisistLogsForPacientAsync(Pacient pacient)
        {
            var res = await WebClient.GetVisitLogAsync(pacient);

            return res;
        }
    }
}
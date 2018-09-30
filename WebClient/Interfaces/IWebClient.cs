using Core.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebClient.Interfaces
{
    public interface IWebClient
    {
        Task<IEnumerable<VisitLog>> GetVisitLogsForPacientAsync(Pacient pacient);

        Task GetProgrammTokenAsync();

        Task<IEnumerable<Pacient>> GetPacientsAsync();

        Task<IEnumerable<Doctor>> GetDoctorsAsync();

        Task SavePacientAsync(Pacient pacient);

        Task SavePacientsDocumentAsync(int pacientId, Document doc);

        Task<Document> GetPacientDocumentAsync(Pacient pacient);

        Task SavePacientVisitAsync(Pacient pacient, Doctor doc);

        Task UpdatePacientsDataAsync(Pacient pacient);

        Task DeleteToken();
    }
}
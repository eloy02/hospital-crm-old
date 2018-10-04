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

        Task<bool> SavePacientAsync(Pacient pacient);

        Task<bool> SavePacientsDocumentAsync(int pacientId, Document doc);

        Task<Document> GetPacientDocumentAsync(Pacient pacient);

        Task<bool> SavePacientVisitAsync(Pacient pacient, Doctor doc);

        Task<bool> UpdatePacientsDataAsync(Pacient pacient);

        Task DeleteToken();

        Task<bool> UpdatePacientDocumentAsync(string filePath, Pacient pacient);
    }
}
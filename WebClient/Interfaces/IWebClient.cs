using Core.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebClient.Interfaces
{
    public interface IWebClient
    {
        Task<IEnumerable<VisitLog>> GetVisitLogAsync(Pacient pacient);

        Task<bool> GetProgrammTokenAsync(User user = null, string password = null);

        Task<IEnumerable<Pacient>> GetPacientsAsync();

        Task<IEnumerable<Doctor>> GetDoctorsAsync();

        Task<bool> SavePacientAsync(Pacient pacient);

        Task<bool> SavePacientsDocumentAsync(int pacientId, Document doc);

        Task<string> GetPacientDocumentAsync(Pacient pacient);

        Task<bool> SavePacientVisitAsync(Pacient pacient, Doctor doc, DateTime visitDateTime);

        Task<bool> UpdatePacientsDataAsync(Pacient pacient);

        Task DeleteToken();

        Task<bool> UpdatePacientDocumentAsync(string filePath, Pacient pacient);

        Task<IEnumerable<User>> GetUsersAsync();

        Task<IEnumerable<VisitLog>> GetVisitLogAsync(Doctor doctor);
    }
}
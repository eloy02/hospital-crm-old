using Core.Types;
using Core.Types.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebClient.Interfaces
{
    public interface IWebClient
    {
        Task<IEnumerable<VisitLog>> GetVisitLogAsync(Pacient pacient);

        Task<(bool requestResult, EAccessGroup? accessGroup)> GetProgrammTokenAsync(User user = null, string password = null);

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

        Task<User> AddUserAsync(User user, string password);

        Task<User> UpdateUserAsync(User user);

        Task<bool> DeleteUserAsync(User user);

        Task<Doctor> AddDoctorAsync(Doctor doctor);

        Task<Doctor> UpdateDoctorAsync(Doctor doctor);

        Task<bool> DeleteDoctorAsync(Doctor doctor);

        Task<bool> DeletePacientAsync(Pacient pacient);
    }
}
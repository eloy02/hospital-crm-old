using Core.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICore
    {
        Task SavePacientAsync(Pacient pacient);

        Task<IEnumerable<Pacient>> GetAllPacientsAsync();

        Task ShowPdfDocumentAsync(Pacient pacient);

        string GetProgrammTempFolder();

        Task<IEnumerable<Doctor>> GetDoctorsAsync();

        Task SetPacientVisit(Pacient pacient, Doctor doctor);

        Task UpdatePacientData(Pacient pacient);
    }
}
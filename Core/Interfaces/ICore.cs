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
    }
}
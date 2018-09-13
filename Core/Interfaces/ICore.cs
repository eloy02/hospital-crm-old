using Core.Types;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICore
    {
        Task SavePacientAsync(PacientCore pacient);
    }
}
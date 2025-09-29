using Napps.Windows.Assessment.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Repositories
{
    public interface IPresentationRepository
    {
        Task<IEnumerable<Presentation>> GetAllAsync();
    }
}
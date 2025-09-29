using Napps.Windows.Assessment.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Repositories.Presentations.Interfaces
{
    public interface IPresentationReader
    {
        Task<IEnumerable<Presentation>> LoadAsync();
    }
}
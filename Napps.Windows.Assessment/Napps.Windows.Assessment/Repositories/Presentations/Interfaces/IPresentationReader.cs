using Napps.Windows.Assessment.Domain;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Repositories.Presentations.Interfaces
{
    public interface IPresentationReader
    {
        Task<PresentationsLoadResult> LoadAsync();
    }
}
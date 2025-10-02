using Napps.Windows.Assessment.Domain.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Repositories.Presentations.Interfaces
{
    public interface IPresentationReader
    {
        Task<PresentationsLoadResult> LoadAsync(CancellationToken cancellationToken);
    }
}
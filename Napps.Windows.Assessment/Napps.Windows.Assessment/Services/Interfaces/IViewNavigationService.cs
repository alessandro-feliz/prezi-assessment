using Napps.Windows.Assessment.Domain.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Services.Interfaces
{
    public interface IViewNavigationService
    {
        Task ShowPresentationListViewAsync(CancellationToken cancellationToken = default);
        Task ShowPresentationDetailsViewAsync(Presentation selectedItem, CancellationToken cancellationToken = default);
    }
}
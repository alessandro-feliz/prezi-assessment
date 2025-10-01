using Caliburn.Micro;
using Napps.Windows.Assessment.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.ViewModels
{
    public interface IMainViewModel
    {
        Task ShowPresentationListView(CancellationToken cancellationToken = default);
        Task ShowPresentationDetailsView(object selectedItem, CancellationToken cancellationToken = default);
    }

    internal class MainViewModel : Conductor<IScreen>.Collection.OneActive, IMainViewModel
    {
        private readonly SimpleContainer _container;

        public MainViewModel(SimpleContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await ShowPresentationListView(cancellationToken);
            await base.OnActivateAsync(cancellationToken);
        }

        public async Task ShowPresentationListView(CancellationToken cancellationToken = default)
        {
            var listVm = _container.GetInstance<IPresentationListViewModel>();
            await ActivateItemAsync(listVm, cancellationToken);
        }

        public async Task ShowPresentationDetailsView(object selectedItem, CancellationToken cancellationToken = default)
        {
            var detailVm = _container.GetInstance<IPresentationDetailViewModel>();
            detailVm.Presentation = selectedItem as Presentation;
            await ActivateItemAsync(detailVm, cancellationToken);
        }
    }
}
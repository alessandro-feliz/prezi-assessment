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
        Task ShowPresentationDetailsView(Presentation selectedItem, CancellationToken cancellationToken = default);
    }

    internal class MainViewModel : Conductor<IScreen>.Collection.OneActive, IMainViewModel, IHandle<Status>
    {
        private readonly SimpleContainer _container;
        private readonly IEventAggregator _eventAggregator;

        private Status _status;
        public Status Status
        {
            get => _status;
            set
            {
                _status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public MainViewModel(SimpleContainer container, IEventAggregator eventAggregator)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _eventAggregator.SubscribeOnPublishedThread(this);

            await ShowPresentationListView(cancellationToken);
            await base.OnActivateAsync(cancellationToken);
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _eventAggregator.Unsubscribe(this);

            await base.OnDeactivateAsync(close, cancellationToken);
        }

        public async Task ShowPresentationListView(CancellationToken cancellationToken = default)
        {
            var presentationListViewModel = _container.GetInstance<IPresentationListViewModel>();
            await ActivateItemAsync(presentationListViewModel, cancellationToken);
        }

        public async Task ShowPresentationDetailsView(Presentation selectedItem, CancellationToken cancellationToken = default)
        {
            var presentationDetailViewModel = _container.GetInstance<IPresentationDetailViewModel>();
            presentationDetailViewModel.Presentation = selectedItem;
            await ActivateItemAsync(presentationDetailViewModel, cancellationToken);
        }

        public Task HandleAsync(Status status, CancellationToken cancellationToken)
        {
            Status = new Status()
            {
                Message = status.Message,
                ProgressStatus = status.ProgressStatus.HasValue ? status.ProgressStatus.Value : Status?.ProgressStatus
            };

            return Task.CompletedTask;
        }
    }
}
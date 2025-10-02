using Caliburn.Micro;
using Napps.Windows.Assessment.Domain.Model;
using Napps.Windows.Assessment.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.ViewModels
{
    public interface IMainViewModel { }

    internal class MainViewModel : Conductor<IScreen>.Collection.OneActive, IMainViewModel, IBusyIndicatorService, IViewNavigationService, IHandle<Status>
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
                NotifyOfPropertyChange(nameof(Status));
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
            await ShowPresentationListViewAsync(cancellationToken);
            await base.OnActivateAsync(cancellationToken);
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _eventAggregator.Unsubscribe(this);
            await base.OnDeactivateAsync(close, cancellationToken);
        }

        // IViewNavigationService Implementation

        public async Task ShowPresentationListViewAsync(CancellationToken cancellationToken = default)
        {
            var presentationListViewModel = _container.GetInstance<IPresentationListViewModel>();
            await ActivateItemAsync(presentationListViewModel, cancellationToken);
        }

        public async Task ShowPresentationDetailsViewAsync(Presentation selectedItem, CancellationToken cancellationToken = default)
        {
            var presentationDetailViewModel = _container.GetInstance<IPresentationDetailViewModel>();
            presentationDetailViewModel.Presentation = selectedItem;
            await ActivateItemAsync(presentationDetailViewModel, cancellationToken);
        }

        // IHandle<Status> Implementation

        public Task HandleAsync(Status status, CancellationToken cancellationToken)
        {
            Status = new Status()
            {
                Message = status.Message,
                ProgressStatus = status.ProgressStatus ?? Status?.ProgressStatus
            };

            return Task.CompletedTask;
        }

        // IBusyIndicator Implementation

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                NotifyOfPropertyChange(nameof(IsBusy));
            }
        }

        public void Show() => IsBusy = true;
        public void Hide() => IsBusy = false;
    }
}
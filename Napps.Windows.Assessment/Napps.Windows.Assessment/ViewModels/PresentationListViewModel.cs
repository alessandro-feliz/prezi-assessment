using Caliburn.Micro;
using Napps.Windows.Assessment.Domain.Model;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Properties;
using Napps.Windows.Assessment.Repositories.Presentations.Interfaces;
using Napps.Windows.Assessment.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.ViewModels
{
    public interface IPresentationListViewModel : IScreen
    {
        ObservableCollection<Presentation> Presentations { get; }
        Presentation SelectedPresentation { get; set; }
        Task ShowPresentationDetailsAsync();
    }

    internal class PresentationListViewModel : Screen, IPresentationListViewModel
    {
        private readonly ILogger _logger;
        private readonly IViewNavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IBusyIndicatorService _busyIndicator;
        private readonly IPresentationReader _fallbackPresentationRepository;

        public ObservableCollection<Presentation> Presentations { get; } = new ObservableCollection<Presentation>();

        private Presentation _selectedPresentation;
        public Presentation SelectedPresentation
        {
            get => _selectedPresentation;
            set
            {
                _selectedPresentation = value;
                NotifyOfPropertyChange(() => SelectedPresentation);
            }
        }

        public PresentationListViewModel(ILogger logger, IViewNavigationService navigationService, IEventAggregator eventAggregator, IBusyIndicatorService busyIndicator, IPresentationReader fallbackPresentationRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _busyIndicator = busyIndicator ?? throw new ArgumentNullException(nameof(busyIndicator));
            _fallbackPresentationRepository = fallbackPresentationRepository ?? throw new ArgumentNullException(nameof(fallbackPresentationRepository));
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await InitializeAsync(cancellationToken);
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _busyIndicator.Show();

            await _eventAggregator.PublishOnUIThreadAsync(new Status() { Message = Resources.StatusDownloadingPresentations });

            try
            {
                var presentationsLoadResult = await _fallbackPresentationRepository.LoadAsync(cancellationToken);

                Presentations.Clear();
                foreach (var presentation in presentationsLoadResult.Presentations.OrderByDescending(p => p.LastModified))
                    Presentations.Add(presentation);

                if (presentationsLoadResult.Mode == Mode.Online)
                {
                    await _eventAggregator.PublishOnUIThreadAsync(new Status() { Message = Resources.StatusOnline, ProgressStatus = ProgressStatus.Online });
                }
                else
                {
                    await _eventAggregator.PublishOnUIThreadAsync(new Status() { Message = Resources.StatusOffline, ProgressStatus = ProgressStatus.Offline });
                }
            }
            catch (FileNotFoundException)
            {
                await _eventAggregator.PublishOnUIThreadAsync(new Status() { Message = Resources.StatusOfflineAndNoCache, ProgressStatus = ProgressStatus.Error });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unhandled exception while fetching presentations");

                await _eventAggregator.PublishOnUIThreadAsync(new Status() { Message = Resources.StatusError, ProgressStatus = ProgressStatus.Error });
            }
            finally
            {
                _busyIndicator.Hide();
            }
        }

        public async Task ShowPresentationDetailsAsync()
        {
            await _navigationService.ShowPresentationDetailsViewAsync(SelectedPresentation);
        }
    }
}
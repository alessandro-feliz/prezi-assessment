using Caliburn.Micro;
using Napps.Windows.Assessment.Domain;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Properties;
using Napps.Windows.Assessment.Repositories.Presentations.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.IO;
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
        private readonly IEventAggregator _eventAggregator;
        private readonly IMainViewModel _mainViewModel;
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

        public PresentationListViewModel(ILogger logger, IEventAggregator eventAggregator, IMainViewModel mainViewModel, IPresentationReader fallbackPresentationRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
            _fallbackPresentationRepository = fallbackPresentationRepository ?? throw new ArgumentNullException(nameof(fallbackPresentationRepository));
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            await _eventAggregator.PublishOnUIThreadAsync(new Status() { Message = Resources.StatusDownloadingPresentations });

            try
            {
                var presentationsLoadResult = await _fallbackPresentationRepository.LoadAsync();

                Presentations.Clear();
                foreach (var presentation in presentationsLoadResult.Presentations)
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
        }

        public async Task ShowPresentationDetailsAsync()
        {
            if (SelectedPresentation != null)
                await _mainViewModel.ShowPresentationDetailsView(SelectedPresentation);
        }
    }
}
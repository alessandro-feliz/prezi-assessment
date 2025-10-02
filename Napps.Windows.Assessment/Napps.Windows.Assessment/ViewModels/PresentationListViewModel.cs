using Caliburn.Micro;
using Napps.Windows.Assessment.Domain.Model;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Properties;
using Napps.Windows.Assessment.Repositories.Presentations.Interfaces;
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

    internal class PresentationListViewModel : BaseViewModel, IPresentationListViewModel
    {
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

        public PresentationListViewModel(ILogger logger, IEventAggregator eventAggregator, IMainViewModel mainViewModel, IPresentationReader fallbackPresentationRepository) : base(logger, eventAggregator)
        {
            _mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
            _fallbackPresentationRepository = fallbackPresentationRepository ?? throw new ArgumentNullException(nameof(fallbackPresentationRepository));
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await InitializeAsync(cancellationToken);
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            await NotifyStatusAsync(Resources.StatusDownloadingPresentations);

            try
            {
                var presentationsLoadResult = await _fallbackPresentationRepository.LoadAsync(cancellationToken);

                Presentations.Clear();
                foreach (var presentation in presentationsLoadResult.Presentations.OrderByDescending(p => p.LastModified))
                    Presentations.Add(presentation);

                if (presentationsLoadResult.Mode == Mode.Online)
                {

                    await NotifyStatusAsync(Resources.StatusOnline, ProgressStatus.Online);
                }
                else
                {
                    await NotifyStatusAsync(Resources.StatusOffline, ProgressStatus.Offline);
                }
            }
            catch (FileNotFoundException)
            {
                await NotifyStatusAsync(Resources.StatusOfflineAndNoCache, ProgressStatus.Error);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unhandled exception while fetching presentations");

                await NotifyStatusAsync(Resources.StatusError, ProgressStatus.Error);
            }
        }

        public async Task ShowPresentationDetailsAsync()
        {
            if (SelectedPresentation != null)
                await _mainViewModel.ShowPresentationDetailsView(SelectedPresentation);
        }
    }
}
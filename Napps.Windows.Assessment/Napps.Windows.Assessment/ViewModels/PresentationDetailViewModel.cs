using Caliburn.Micro;
using Napps.Windows.Assessment.Domain;
using Napps.Windows.Assessment.Logger;
using System;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.ViewModels
{
    public interface IPresentationDetailViewModel : IScreen
    {
        Presentation Presentation { get; set; }
        Task ShowPresentationListAsync();
    }

    internal class PresentationDetailViewModel : Screen, IPresentationDetailViewModel
    {
        private readonly ILogger _logger;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMainViewModel _mainViewModel;

        private Presentation _presentation;
        public Presentation Presentation
        {
            get => _presentation;
            set
            {
                _presentation = value;
                NotifyOfPropertyChange(() => Presentation);
            }
        }

        public PresentationDetailViewModel(ILogger logger, IEventAggregator eventAggregator, IMainViewModel mainViewModel)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
        }

        public async Task ShowPresentationListAsync()
        {
            await _mainViewModel.ShowPresentationListView();
        }
    }
}
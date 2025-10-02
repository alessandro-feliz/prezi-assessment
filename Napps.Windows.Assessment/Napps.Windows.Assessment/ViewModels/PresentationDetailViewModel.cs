using Caliburn.Micro;
using Napps.Windows.Assessment.Domain.Model;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.ViewModels
{
    public interface IPresentationDetailViewModel : IScreen
    {
        Presentation Presentation { get; set; }
        Task ShowPresentationListAsync();
    }

    public class PresentationDetailViewModel : Screen, IPresentationDetailViewModel
    {
        private readonly ILogger _logger;
        private readonly IViewNavigationService _navigationService;

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

        public PresentationDetailViewModel(ILogger logger, IViewNavigationService navigationService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _logger.Info($"Accessing presentation {Presentation.Id}");
            return Task.CompletedTask;
        }

        public async Task ShowPresentationListAsync()
        {
            await _navigationService.ShowPresentationListViewAsync();
        }
    }
}
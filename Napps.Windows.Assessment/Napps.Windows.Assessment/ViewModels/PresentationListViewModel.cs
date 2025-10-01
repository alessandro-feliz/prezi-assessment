using Caliburn.Micro;
using Napps.Windows.Assessment.Domain;
using Napps.Windows.Assessment.Repositories.Presentations.Interfaces;
using System;
using System.Collections.ObjectModel;
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
        private readonly IMainViewModel _mainViewModel;
        private readonly IPresentationReader _fallbackPresentationRepository;

        public ObservableCollection<Presentation> Presentations { get; } = new ObservableCollection<Presentation>();

        public Presentation SelectedPresentation { get; set; }

        public PresentationListViewModel(IMainViewModel mainViewModel, IPresentationReader fallbackPresentationRepository)
        {
            _mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
            _fallbackPresentationRepository = fallbackPresentationRepository ?? throw new ArgumentNullException(nameof(fallbackPresentationRepository));
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            var presentations = await _fallbackPresentationRepository.LoadAsync();

            Presentations.Clear();
            foreach (var presentation in presentations)
                Presentations.Add(presentation);
        }

        public async Task ShowPresentationDetailsAsync()
        {
            await _mainViewModel.ShowPresentationDetailsView(SelectedPresentation);
        }
    }
}
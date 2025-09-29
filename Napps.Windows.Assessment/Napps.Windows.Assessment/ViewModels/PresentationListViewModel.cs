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

    }

    internal class PresentationListViewModel : Screen, IPresentationListViewModel
    {
        private readonly IPresentationReader _fallbackPresentationRepository;

        public ObservableCollection<Presentation> Presentations { get; set; } = new ObservableCollection<Presentation>();

        public PresentationListViewModel(IPresentationReader fallbackPresentationRepository)
        {
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
    }
}
using Caliburn.Micro;
using Napps.Windows.Assessment.Domain;
using Napps.Windows.Assessment.Repositories;
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
        private readonly IPresentationRepository _presentationRepository;

        public ObservableCollection<Presentation> Presentations { get; set; } = new ObservableCollection<Presentation>();

        public PresentationListViewModel(IPresentationRepository presentationRepository)
        {
            _presentationRepository = presentationRepository ?? throw new ArgumentNullException(nameof(presentationRepository));
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            var presentations = await _presentationRepository.GetAllAsync();

            Presentations.Clear();
            foreach (var presentation in presentations)
                Presentations.Add(presentation);
        }
    }
}
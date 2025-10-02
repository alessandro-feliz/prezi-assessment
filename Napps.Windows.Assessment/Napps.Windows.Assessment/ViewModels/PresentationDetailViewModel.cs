using Caliburn.Micro;
using Napps.Windows.Assessment.Domain.Model;
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

    internal class PresentationDetailViewModel : BaseViewModel, IPresentationDetailViewModel
    {
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

        public PresentationDetailViewModel(ILogger logger, IEventAggregator eventAggregator, IMainViewModel mainViewModel) : base(logger, eventAggregator)
        {
            _mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
        }

        public async Task ShowPresentationListAsync()
        {
            await _mainViewModel.ShowPresentationListView();
        }
    }
}
using Caliburn.Micro;
using Napps.Windows.Assessment.Domain;
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
        private readonly IMainViewModel _mainViewModel;

        public Presentation Presentation { get; set; }

        public PresentationDetailViewModel(IMainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
        }

        public async Task ShowPresentationListAsync()
        {
            await _mainViewModel.ShowPresentationListView();
        }
    }
}
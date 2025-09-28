using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Napps.Windows.Assessment.ViewModels
{
	public interface IMainViewModel
	{

	}

	internal class MainViewModel : Conductor<IScreen>.Collection.OneActive, IMainViewModel
	{
		private readonly IPresentationListViewModel presentationListViewModel;

		public MainViewModel(IPresentationListViewModel presentationListViewModel)
		{
			this.presentationListViewModel = presentationListViewModel;
		}

		protected override async Task OnActivateAsync(CancellationToken cancellationToken)
		{
			await ActivateItemAsync(presentationListViewModel, cancellationToken);
			await base.OnActivateAsync(cancellationToken);
		}
	}
}

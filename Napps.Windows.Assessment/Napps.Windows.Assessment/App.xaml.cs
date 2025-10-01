using Napps.Windows.Assessment.Utils;
using System.Windows;

namespace Napps.Windows.Assessment
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private Bootstrapper bootstrapper;

		protected override void OnStartup(StartupEventArgs e)
		{
			bootstrapper = new Bootstrapper();

			base.OnStartup(e);
		}
	}
}
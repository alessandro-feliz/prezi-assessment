using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Napps.Windows.Assessment.Utils;

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

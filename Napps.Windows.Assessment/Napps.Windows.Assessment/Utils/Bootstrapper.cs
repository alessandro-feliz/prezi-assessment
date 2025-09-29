using Caliburn.Micro;
using Napps.Windows.Assessment.Configuration;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Repositories;
using Napps.Windows.Assessment.Repositories.Impl;
using Napps.Windows.Assessment.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Threading;

namespace Napps.Windows.Assessment.Utils
{
	internal class Bootstrapper : BootstrapperBase
	{
		private SimpleContainer container;

		public Bootstrapper()
		{
			Initialize();
		}

		protected override void Configure()
		{
			container = new SimpleContainer();

			var config = ConfigManager.LoadFromFile();
            container.Instance(config);

            container.Singleton<ILogger, NLogger>();

            container.Instance(new HttpClient() { Timeout = config.PresentationsEndpointTimeout });

			container.Singleton<IWindowManager, WindowManager>();
			container.Singleton<ICrashReporter, CrashReporter>();

            container.Singleton<IPresentationRepository, ApiPresentationRepository>();

			container.Singleton<IMainViewModel, MainViewModel>();
			container.PerRequest<IPresentationListViewModel, PresentationListViewModel>();

            container.Handler<IDependencyContainer>(simpleContainer => new DependencyContainer(simpleContainer));
		}

		protected override object GetInstance(Type service, string key)
		{
			return container.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			container.BuildUp(instance);
		}

		protected override async void OnStartup(object sender, StartupEventArgs e)
		{
			await DisplayRootViewForAsync<IMainViewModel>();
		}

		protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			base.OnUnhandledException(sender, e);
		}
	}
}
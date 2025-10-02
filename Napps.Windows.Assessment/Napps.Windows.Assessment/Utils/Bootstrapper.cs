using Caliburn.Micro;
using Napps.Windows.Assessment.Configuration;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Repositories.Presentations;
using Napps.Windows.Assessment.Repositories.Presentations.Interfaces;
using Napps.Windows.Assessment.Services;
using Napps.Windows.Assessment.Services.Interfaces;
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
            container.Instance(container);

            // Core
            container.Singleton<ILogger, NLogger>();
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<ICrashReporter, CrashReporter>();
            container.Singleton<IConfigProvider, ConfigProvider>();

            // Config
            var config = container.GetInstance<IConfigProvider>().Load();
            container.Instance(config);

            // HttpClient
            container.Instance(new HttpClient() { Timeout = config.PresentationsEndpointTimeout });

            // Repos and Services
            container.Singleton<PresentationApiRepository>();
            container.Singleton<PresentationFileRepository>();
            container.Singleton<IJsonSerializerService, JsonSerializerService>();
            container.Singleton<IFileSerializerService, BinarySerializerService>();
            container.Singleton<IThumbnailService, ThumbnailService>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Handler<IPresentationWriter>(c => c.GetInstance<PresentationFileRepository>());
            container.Handler<IPresentationReader>(c => new PresentationFallbackRepository(c.GetInstance<ILogger>(), c.GetInstance<PresentationApiRepository>(), c.GetInstance<PresentationFileRepository>()));

            // Views
            container.Singleton<IMainViewModel, MainViewModel>();
            container.PerRequest<IPresentationListViewModel, PresentationListViewModel>();
            container.PerRequest<IPresentationDetailViewModel, PresentationDetailViewModel>();

            container.Handler<IBusyIndicatorService>(c => c.GetInstance<IMainViewModel>());
            container.Handler<IViewNavigationService>(c => c.GetInstance<IMainViewModel>());

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
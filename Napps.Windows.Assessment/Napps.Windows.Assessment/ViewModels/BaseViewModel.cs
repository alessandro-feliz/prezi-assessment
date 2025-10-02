using Caliburn.Micro;
using Napps.Windows.Assessment.Domain.Model;
using Napps.Windows.Assessment.Logger;
using System;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.ViewModels
{
    internal class BaseViewModel : Screen
    {
        protected readonly ILogger _logger;
        private readonly IEventAggregator _eventAggregator;

        public BaseViewModel(ILogger logger, IEventAggregator eventAggregator) : base()
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        }

        protected async Task NotifyStatusAsync(string message, ProgressStatus? progressStatus = null)
        {
            await _eventAggregator.PublishOnUIThreadAsync(new Status() { Message = message, ProgressStatus = progressStatus });
        }
    }
}
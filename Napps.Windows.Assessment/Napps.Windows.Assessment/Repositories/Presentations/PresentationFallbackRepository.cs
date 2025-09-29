using Napps.Windows.Assessment.Domain;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Repositories.Presentations.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Repositories.Presentations
{
    internal class PresentationFallbackRepository : IPresentationReader
    {
        private readonly ILogger _logger;
        private readonly IPresentationReader _presentationApiRepository;
        private readonly IPresentationReader _presentationFileRepository;

        public PresentationFallbackRepository(ILogger logger, IPresentationReader presentationApiRepository, IPresentationReader presentationFileRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _presentationApiRepository = presentationApiRepository ?? throw new ArgumentNullException(nameof(presentationApiRepository));
            _presentationFileRepository = presentationFileRepository ?? throw new ArgumentNullException(nameof(presentationFileRepository));
        }

        public async Task<IEnumerable<Presentation>> LoadAsync()
        {
            try
            {
                return await _presentationApiRepository.LoadAsync();
            }
            catch
            {
                _logger.Warn("Couldn't fetch presentations from API, using fallback mode");

                return await _presentationFileRepository.LoadAsync();
            }
        }
    }
}
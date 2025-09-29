using Napps.Windows.Assessment.Configuration;
using Napps.Windows.Assessment.Domain;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Repositories.Presentations.Interfaces;
using Napps.Windows.Assessment.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Repositories.Presentations
{
    internal class PresentationFileRepository : IPresentationReader, IPresentationWriter
    {
        private readonly ILogger _logger;
        private readonly Config _config;
        private readonly IFileSerializerService _serializerService;
        private readonly string _presentationsFile;

        public PresentationFileRepository(ILogger logger, Config config, IFileSerializerService serializerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _serializerService = serializerService ?? throw new ArgumentNullException(nameof(serializerService));

            _presentationsFile = Path.Combine(_config.ApplicationFolder, _config.PresentationslFile);
        }

        public async Task<IEnumerable<Presentation>> LoadAsync()
        {
            if (!File.Exists(_presentationsFile))
            {
                _logger.Warn("No fallback file found");
                return null;
            }

            return await _serializerService.DeserializeAsync<IEnumerable<Presentation>>(_presentationsFile);
        }

        public async Task SaveAsync(IEnumerable<Presentation> presentations)
        {
            await _serializerService.SerializeAsync(presentations, _presentationsFile);
        }
    }
}
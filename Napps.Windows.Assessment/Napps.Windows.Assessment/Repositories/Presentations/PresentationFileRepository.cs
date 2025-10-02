using Napps.Windows.Assessment.Configuration;
using Napps.Windows.Assessment.Domain.Model;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Repositories.Presentations.Interfaces;
using Napps.Windows.Assessment.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

            _presentationsFile = Path.Combine(_config.ApplicationFolder, _config.PresentationsFile);
        }

        public async Task<PresentationsLoadResult> LoadAsync(CancellationToken cancellationToken)
        {
            if (!File.Exists(_presentationsFile))
            {
                _logger.Error($"File {_presentationsFile} not found");

                throw new FileNotFoundException($"File {_presentationsFile} not found");
            }

            var presentations = await _serializerService.DeserializeAsync<IEnumerable<Presentation>>(_presentationsFile, cancellationToken);

            return new PresentationsLoadResult(Mode.Offline, presentations);
        }

        public async Task SaveAsync(IEnumerable<Presentation> presentations, CancellationToken cancellationToken)
        {
            await _serializerService.SerializeAsync(presentations, _presentationsFile, cancellationToken);
        }
    }
}
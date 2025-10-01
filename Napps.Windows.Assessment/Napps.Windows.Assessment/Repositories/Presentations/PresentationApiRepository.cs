using Napps.Windows.Assessment.Configuration;
using Napps.Windows.Assessment.Domain;
using Napps.Windows.Assessment.Dto;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Repositories.Presentations.Interfaces;
using Napps.Windows.Assessment.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Repositories.Presentations
{
    internal class PresentationApiRepository : IPresentationReader
    {
        private readonly ILogger _logger;
        private readonly Config _config;
        private readonly HttpClient _httpClient;
        private readonly IJsonSerializerService _serializerService;
        private readonly IThumbnailService _thumbnailService;
        private readonly IPresentationWriter _writer;

        public PresentationApiRepository(ILogger logger, Config config, HttpClient httpClient, IJsonSerializerService serializer, IThumbnailService thumbnailService, IPresentationWriter writer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _serializerService = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _thumbnailService = thumbnailService ?? throw new ArgumentNullException(nameof(thumbnailService));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public async Task<PresentationsLoadResult> LoadAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_config.PresentationsEndpoint);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var presentations = new List<Presentation>();

                var presentationsDto = await _serializerService.DeserializeAsync<PresentationsDto>(json);

                foreach (var p in presentationsDto.Presentations)
                {
                    presentations.Add(
                          new Presentation(
                              p.Id,
                              p.Title,
                              p.ThumbnailUrl,
                              await _thumbnailService.DownloadAndSaveAsync(p.ThumbnailUrl, p.Id),
                              Enum.TryParse<Privacy>(p.Privacy, out var parsedPrivacy) ? parsedPrivacy : Privacy.Hidden,
                              p.LastModified,
                              new Author(p.Owner.Id, p.Owner.FirstName, p.Owner.LastName),
                              p.Description
                          )
                      );
                }

                await _writer.SaveAsync(presentations);

                return new PresentationsLoadResult(Mode.Online, presentations);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error fetching presentations from API");

                throw ex;
            }
        }
    }
}
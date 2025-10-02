using Napps.Windows.Assessment.Configuration;
using Napps.Windows.Assessment.Domain.Model;
using Napps.Windows.Assessment.Domain.Dto;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Repositories.Presentations.Interfaces;
using Napps.Windows.Assessment.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
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

        public async Task<PresentationsLoadResult> LoadAsync(CancellationToken cancellationToken)
        {
            try
            {
                var presentationsDto = await FeatchPresentationsDtoAsync(cancellationToken);

                var downloadedThumbnailPaths = await DownloadPresentationThumbnails(presentationsDto, cancellationToken);

                var presentations = BuildPresentations(presentationsDto, downloadedThumbnailPaths);

                await _writer.SaveAsync(presentations, cancellationToken);

                return new PresentationsLoadResult(Mode.Online, presentations);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error fetching presentations from API");

                throw;
            }
        }

        private static IEnumerable<Presentation> BuildPresentations(PresentationsDto presentationsDto, string[] downloadedThumbnailPaths)
        {
            var presentations = new List<Presentation>();

            for (int i = 0; i < presentationsDto.Presentations.Count; i++)
            {
                var p = presentationsDto.Presentations[i];

                presentations.Add(
                      new Presentation(
                          p.Id,
                          p.Title,
                          p.ThumbnailUrl,
                          downloadedThumbnailPaths[i],
                          Enum.TryParse<Privacy>(p.Privacy, out var parsedPrivacy) ? parsedPrivacy : Privacy.Hidden,
                          p.LastModified,
                          new Author(p.Owner.Id, p.Owner.FirstName, p.Owner.LastName),
                          p.Description
                      )
                  );
            }

            return presentations;
        }

        private async Task<string[]> DownloadPresentationThumbnails(PresentationsDto presentationsDto, CancellationToken cancellationToken)
        {
            var downloadThumbnailTasks = new List<Task<string>>();

            foreach (var presentationDto in presentationsDto.Presentations)
            {
                downloadThumbnailTasks.Add(_thumbnailService.DownloadAndSaveAsync(presentationDto.ThumbnailUrl, presentationDto.Id, cancellationToken));
            }

            return await Task.WhenAll(downloadThumbnailTasks);
        }

        private async Task<PresentationsDto> FeatchPresentationsDtoAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(_config.PresentationsEndpoint, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return await _serializerService.DeserializeAsync<PresentationsDto>(json, cancellationToken);
        }
    }
}
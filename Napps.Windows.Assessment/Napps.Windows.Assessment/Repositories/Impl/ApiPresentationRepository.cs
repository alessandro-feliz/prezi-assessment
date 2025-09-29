using Napps.Windows.Assessment.Configuration;
using Napps.Windows.Assessment.Domain;
using Napps.Windows.Assessment.Dto;
using Napps.Windows.Assessment.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Repositories.Impl
{
    public class ApiPresentationRepository : IPresentationRepository
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly Config _config;

        public ApiPresentationRepository(ILogger logger, HttpClient httpClient, Config config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<IEnumerable<Presentation>> GetAllAsync()
        {
            try
            {
                using (var response = await _httpClient.GetAsync(_config.PresentationsEndpoint))
                {
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    var dto = JsonConvert.DeserializeObject<PresentationsDto>(json);

                    if (dto?.Presentations == null)
                        return Enumerable.Empty<Presentation>();

                    return dto.Presentations.Select(p =>
                        new Presentation(
                            id: p.Id,
                            title: p.Title,
                            thumbnailUrl: p.ThumbnailUrl,
                            privacy: Enum.TryParse<Privacy>(p.Privacy, out var parsedPrivacy) ? parsedPrivacy : Privacy.Public,
                            lastModified: p.LastModified,
                            author: new Author(p.Owner.Id, p.Owner.FirstName, p.Owner.LastName)
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error fetching presentations");
                return Enumerable.Empty<Presentation>();
            }
        }
    }
}
using Napps.Windows.Assessment.Configuration;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Services.Interfaces;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Services
{
    internal class ThumbnailService : IThumbnailService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly string _thumbnailDirectory;

        private readonly string _defaultImagePath;

        public ThumbnailService(HttpClient httpClient, ILogger logger, Config config)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _thumbnailDirectory = Path.Combine(config.ApplicationFolder, config.ThumbnailFolder);
            if (!Directory.Exists(_thumbnailDirectory))
                Directory.CreateDirectory(_thumbnailDirectory);

            _defaultImagePath = Path.Combine(config.ApplicationFolder, "Assets", "prezi_logo.png");
        }

        public async Task<string> DownloadAndSaveAsync(string thumbnailUrl, string thumbnailName)
        {
            if (string.IsNullOrWhiteSpace(thumbnailUrl)) throw new ArgumentNullException(nameof(thumbnailUrl));
            if (string.IsNullOrWhiteSpace(thumbnailName)) throw new ArgumentNullException(nameof(thumbnailName));

            try
            {
                var extension = Path.GetExtension(thumbnailUrl)?.Split('?')[0] ?? ".jpg";
                var fileName = $"{thumbnailName}{extension}";
                var filePath = Path.Combine(_thumbnailDirectory, fileName);

                if (File.Exists(filePath))
                    return filePath;

                var bytes = await _httpClient.GetByteArrayAsync(thumbnailUrl);

                File.WriteAllBytes(filePath, bytes);

                return filePath;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to download thumbnail for presentation {thumbnailName} from {thumbnailUrl}");

                if (!File.Exists(_defaultImagePath))
                {
                    _logger.Error($"Default image {_defaultImagePath} not found");

                    return string.Empty;
                }

                return _defaultImagePath;
            }
        }
    }
}
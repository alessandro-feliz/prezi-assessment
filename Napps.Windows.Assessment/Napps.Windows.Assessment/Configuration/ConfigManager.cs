using System;
using System.Configuration;
using System.IO;

namespace Napps.Windows.Assessment.Configuration
{
    public class ConfigManager
    {
        private const string PresentationsEndpointKey = "PresentationsEndpoint";
        private const string PresentationsEndpointTimeoutSecsKey = "PresentationsEndpointTimeoutSecs";

        public static Config LoadFromFile()
        {
            var presentationsEndpoint = ConfigurationManager.AppSettings[PresentationsEndpointKey];
            if (string.IsNullOrWhiteSpace(presentationsEndpoint))
                throw new ConfigurationErrorsException($"Missing configuration {PresentationsEndpointKey}");

            var timeoutStr = ConfigurationManager.AppSettings[PresentationsEndpointTimeoutSecsKey];
            if (!int.TryParse(timeoutStr, out var timeoutSecs) || timeoutSecs <= 0)
                throw new ConfigurationErrorsException($"Missing or invalid configuration {PresentationsEndpointTimeoutSecsKey}");

            return new Config()
            {
                PresentationsEndpoint = presentationsEndpoint,
                PresentationsEndpointTimeout = TimeSpan.FromSeconds(timeoutSecs),
                ApplicationFolder = AppDomain.CurrentDomain.BaseDirectory,
                ThumbnailFolder = "Thumbnails",
                PresentationslFile = Path.Combine("Presentations", "Presentations.bin")
            };
        }
    }
}
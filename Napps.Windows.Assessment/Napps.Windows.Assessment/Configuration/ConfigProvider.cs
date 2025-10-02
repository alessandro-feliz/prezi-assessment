using System;
using System.Configuration;
using System.IO;

namespace Napps.Windows.Assessment.Configuration
{
    public interface IConfigProvider
    {
        Config Load();
    }

    public class ConfigProvider : IConfigProvider
    {
        private const string PresentationsEndpointKey = "PresentationsEndpoint";
        private const string TimeoutSecsKey = "PresentationsEndpointTimeoutSecs";
        private const string ThumbnailFolderKey = "ThumbnailFolder";
        private const string PresentationsFileKey = "PresentationsFile";

        public Config Load()
        {
            return new Config
            {
                PresentationsEndpoint = GetRequiredString(PresentationsEndpointKey),
                PresentationsEndpointTimeout = TimeSpan.FromSeconds(GetRequiredInt(TimeoutSecsKey)),
                ApplicationFolder = AppDomain.CurrentDomain.BaseDirectory,
                ThumbnailFolder = GetString(ThumbnailFolderKey, "Thumbnails"),
                PresentationsFile = GetString(PresentationsFileKey, Path.Combine("Presentations", "Presentations.bin"))
            };
        }

        private string GetRequiredString(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
                throw new ConfigurationErrorsException($"Missing configuration key: {key}");
            return value;
        }

        private string GetString(string key, string defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        private int GetRequiredInt(string key)
        {
            var value = GetRequiredString(key);
            if (!int.TryParse(value, out var result) || result <= 0)
                throw new ConfigurationErrorsException($"Invalid integer for key: {key}");
            return result;
        }
    }
}
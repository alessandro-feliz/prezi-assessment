using System;
using System.Configuration;
using System.IO;

namespace Napps.Windows.Assessment.Configuration
{
    public class ConfigManager
    {
        private const string PresentationsEndpointKey = "PresentationsEndpoint";
        private const string TimeoutSecsKey = "PresentationsEndpointTimeoutSecs";
        private const string ThumbnailFolderKey = "ThumbnailFolder";
        private const string PresentationsFileKey = "PresentationsFile";
             
        public static Config LoadFromFile()
        {
            return new Config
            {
                PresentationsEndpoint = GetRequiredSetting(PresentationsEndpointKey),
                PresentationsEndpointTimeout = TimeSpan.FromSeconds(GetRequiredInt(TimeoutSecsKey)),
                ApplicationFolder = AppDomain.CurrentDomain.BaseDirectory,
                ThumbnailFolder = GetSetting(ThumbnailFolderKey, "Thumbnails"),
                PresentationslFile = GetSetting(PresentationsFileKey, Path.Combine("Presentations", "Presentations.bin"))
            };
        }

        private static string GetRequiredSetting(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
                throw new ConfigurationErrorsException($"Missing configuration key: {key}");
            return value;
        }

        private static string GetSetting(string key, string defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        private static int GetRequiredInt(string key)
        {
            var value = GetRequiredSetting(key);
            if (!int.TryParse(value, out var result) || result <= 0)
                throw new ConfigurationErrorsException($"Invalid integer for key: {key}");
            return result;
        }
    }
}
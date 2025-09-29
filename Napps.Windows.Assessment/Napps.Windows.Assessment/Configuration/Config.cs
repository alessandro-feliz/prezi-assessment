using System;

namespace Napps.Windows.Assessment.Configuration
{
    public class Config
    {
        public string PresentationsEndpoint { get; set; }
        public TimeSpan PresentationsEndpointTimeout { get; set; }
        public string ApplicationFolder { get; set; }
        public string ThumbnailFolder { get; set; }
        public string PresentationslFile { get; set; }
    }
}
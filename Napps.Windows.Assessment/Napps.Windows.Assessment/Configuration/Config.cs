using System;

namespace Napps.Windows.Assessment.Configuration
{
    public class Config
    {
        public string PresentationsEndpoint { get; set; }
        public TimeSpan PresentationsEndpointTimeout { get; set; }
    }
}
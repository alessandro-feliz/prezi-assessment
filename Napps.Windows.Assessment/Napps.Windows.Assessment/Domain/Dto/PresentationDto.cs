using System;

namespace Napps.Windows.Assessment.Domain.Dto
{
    public class PresentationDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Description { get; set; }
        public string Privacy { get; set; }
        public DateTime LastModified { get; set; }
        public OwnerDto Owner { get; set; }
    }
}
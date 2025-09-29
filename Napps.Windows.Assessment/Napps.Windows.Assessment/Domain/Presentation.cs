using System;

namespace Napps.Windows.Assessment.Domain
{        
    public class Presentation
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public Privacy Privacy { get; private set; }
        public DateTime LastModified { get; private set; }
        public Author Author { get; private set; }

        public Presentation(string id, string title, string thumbnailUrl, Privacy privacy, DateTime lastModified, Author author)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            ThumbnailUrl = thumbnailUrl ?? throw new ArgumentNullException(nameof(thumbnailUrl));
            Privacy = privacy;
            LastModified = lastModified;
            Author = author;
        }
    }
}
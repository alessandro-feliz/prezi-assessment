using System;

namespace Napps.Windows.Assessment.Domain
{
    [Serializable]
    public class Presentation
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public string ThumbnaiFile { get; private set; }
        public Privacy Privacy { get; private set; }
        public DateTime LastModified { get; private set; }
        public Author Author { get; private set; }
        public string Description { get; private set; }

        public Presentation(string id, string title, string thumbnailUrl, string thumbnailFile, Privacy privacy, DateTime lastModified, Author author, string description)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            ThumbnailUrl = thumbnailUrl ?? throw new ArgumentNullException(nameof(thumbnailUrl));
            ThumbnaiFile = thumbnailFile ?? throw new ArgumentNullException(nameof(thumbnailFile));
            Privacy = privacy;
            LastModified = lastModified;
            Author = author;
            Description = description ?? String.Empty;
        }
    }
}
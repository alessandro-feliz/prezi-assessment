using System;

namespace Napps.Windows.Assessment.Domain
{
    [Serializable]
    public class Author
    {
        public string Id { get; private set; }

        public Name Name { get; private set; }

        public Author(string id, string firstName, string lastName)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = new Name(firstName, lastName);
        }
    }
}
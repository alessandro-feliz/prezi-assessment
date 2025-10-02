using System;

namespace Napps.Windows.Assessment.Domain.Model
{
    [Serializable]
    public class Name
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string FullName => GetFullName();

        public Name(string firstName, string lastName) 
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException( nameof(lastName));
        }

        private string GetFullName()
        {
            return $"{LastName}, {FirstName}";
        }
    }
}
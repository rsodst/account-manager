using System;

namespace Modulbank.Profiles.Domain
{
    public class PersonPhoto
    {
        public PersonPhoto()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string FileName { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModified { get; set; }
    }
}
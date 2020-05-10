using System;
using Modulbank.Profiles.Specification;

namespace Modulbank.Profiles.Domain
{
    public class PersonDetails : IPersonDetails
    {
        public PersonDetails()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public DateTime LastModified { get; set; }
    }
}
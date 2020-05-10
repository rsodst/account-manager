using System;

namespace Modulbank.Profiles.Domain
{
    public class ProfileConfirmation
    {
        public ProfileConfirmation()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public DateTime LastModified { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public string Description { get; set; }
    }
}
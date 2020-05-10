using Modulbank.Accounts.Enums;

namespace Modulbank.Accounts.Specification
{
    public interface IAccountDetail
    {
        decimal LimitByOperation { get; set; }
        
        string Description { get; set; }

        Currency Currency { get; set; }
    }
}
using MediatR;

namespace Modulbank.FileStorage.Queries
{
    public class GetPhotoContentQuery : IRequest<string>
    {
        public string FileName { get; set; }
    }
}
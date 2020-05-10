using MediatR;

namespace Modulbank.FileStorage.Queries
{
    public class GetPersonPhotoContentQuery : IRequest<string>
    {
        public string FileName { get; set; }
    }
}
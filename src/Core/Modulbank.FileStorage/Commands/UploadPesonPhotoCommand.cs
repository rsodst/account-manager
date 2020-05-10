using MediatR;

namespace Modulbank.FileStorage.Commands
{
    public class UploadPesonPhotoCommand : IRequest<string>
    {
        public string Base64ImageFile { get; set; }
    }
}
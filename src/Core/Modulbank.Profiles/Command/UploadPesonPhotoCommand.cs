using MediatR;

namespace Modulbank.Profiles.Command
{
    public class UploadPesonPhotoCommand : IRequest<string>
    {
        public string Base64ImageFile { get; set; }
    }
}
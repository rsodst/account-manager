using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.Profiles.Command;
using Modulbank.Profiles.Domain;
using Modulbank.Profiles.Services;
using Modulbank.Users.Tables;

namespace Modulbank.Profiles.RequestHandlers
{
    public class UploadPersonPhotoCommandHandler : IRequestHandler<UploadPesonPhotoCommand, string>
    {
        private readonly IPhotoUploaderService _photoUploaderService;

        public UploadPersonPhotoCommandHandler(IPhotoUploaderService photoUploaderService)
        {
            _photoUploaderService = photoUploaderService ?? throw new ArgumentNullException(nameof(photoUploaderService));
        }

        public async Task<string> Handle(UploadPesonPhotoCommand request, CancellationToken cancellationToken)
        {
            // TODO: validate file
            
            return await _photoUploaderService.SaveAsync(request.Base64ImageFile);
        }
    }
}
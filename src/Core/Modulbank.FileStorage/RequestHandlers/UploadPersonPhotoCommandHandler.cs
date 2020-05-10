using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.FileStorage.Commands;
using Modulbank.FileStorage.Services;

namespace Modulbank.FileStorage.RequestHandlers
{
    public class UploadPersonPhotoCommandHandler : IRequestHandler<UploadPesonPhotoCommand, string>
    {
        private readonly IPhotoService _photoService;

        public UploadPersonPhotoCommandHandler(IPhotoService photoService)
        {
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }

        public async Task<string> Handle(UploadPesonPhotoCommand request, CancellationToken cancellationToken)
        {
            // TODO: validate file

            return await _photoService.SaveAsync(request.Base64ImageFile);
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modulbank.FileStorage.Queries;
using Modulbank.FileStorage.Services;

namespace Modulbank.FileStorage.RequestHandlers
{
    public class GetPhotoContentQueryHandler : IRequestHandler<GetPhotoContentQuery, string>
    {
        private readonly IPhotoService _photoService;

        public GetPhotoContentQueryHandler(IPhotoService photoService)
        {
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }

        public async Task<string> Handle(GetPhotoContentQuery request, CancellationToken cancellationToken)
        {
            return await _photoService.GetAsync(request.FileName);
        }
    }
}
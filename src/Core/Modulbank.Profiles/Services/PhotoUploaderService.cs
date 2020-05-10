using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Modulbank.Settings;

namespace Modulbank.Profiles.Services
{
    public interface IPhotoUploaderService
    {
        Task<string> SaveAsync(string base64Image);
    }

    public class PhotoUploaderService : IPhotoUploaderService
    {
        private readonly FileStorageOptions _fileStorageOptions;

        public PhotoUploaderService(IOptions<FileStorageOptions> fileStorageOptions)
        {
            _fileStorageOptions = fileStorageOptions.Value ?? throw new ArgumentNullException(nameof(fileStorageOptions));
        }

        public async Task<string> SaveAsync(string base64Image)
        {
            var bytes = Convert.FromBase64String(base64Image);

            var fileName = $"{Guid.NewGuid():N}.jpg";

            await File.WriteAllBytesAsync(Path.Combine(_fileStorageOptions.PersonPhotoPath, fileName), bytes);

            return fileName;
        }
    }
}
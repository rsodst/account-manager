using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Modulbank.Settings;
using Modulbank.Shared.Exceptions;

namespace Modulbank.FileStorage.Services
{
    public interface IPhotoService
    {
        Task<string> SaveAsync(string base64Image);

        Task<string> GetAsync(string fileName);
    }

    public class PhotoService : IPhotoService
    {
        private readonly FileStorageOptions _fileStorageOptions;

        public PhotoService(IOptions<FileStorageOptions> fileStorageOptions)
        {
            _fileStorageOptions = fileStorageOptions.Value ?? throw new ArgumentNullException(nameof(fileStorageOptions));

            if (Directory.Exists(_fileStorageOptions.PersonPhotoPath) == false)
            {
                Directory.CreateDirectory(_fileStorageOptions.PersonPhotoPath);
            }
        }

        public async Task<string> SaveAsync(string base64Image)
        {
            var bytes = Convert.FromBase64String(base64Image);

            var fileName = $"{Guid.NewGuid():N}.jpg";

            await File.WriteAllBytesAsync(Path.Combine(_fileStorageOptions.PersonPhotoPath, fileName), bytes);

            return fileName;
        }
        
        public async Task<string> GetAsync(string fileName)
        {
            if (File.Exists(Path.Combine(_fileStorageOptions.PersonPhotoPath, fileName)) == false)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, $"File {fileName} not found");
            }

            var bytes = await File.ReadAllBytesAsync(Path.Combine(_fileStorageOptions.PersonPhotoPath, fileName));

            var base64String = Convert.ToBase64String(bytes);

            return base64String;
        }
    }
}
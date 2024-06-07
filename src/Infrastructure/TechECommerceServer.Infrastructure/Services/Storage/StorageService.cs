using Microsoft.AspNetCore.Http;
using TechECommerceServer.Application.Abstractions.Storage;

namespace TechECommerceServer.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        private readonly IStorage _storage;
        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName { get => _storage.GetType().Name; }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadFileAsync(string pathOrContainerName, IFormFileCollection files)
            => await _storage.UploadFileAsync(pathOrContainerName, files);

        public async Task DeleteFileAsync(string pathOrContainerName, string fileName)
            => await _storage.DeleteFileAsync(pathOrContainerName, fileName);

        public List<string> GetFiles(string pathOrContainerName)
            => _storage.GetFiles(pathOrContainerName);

        public bool HasFile(string pathOrContainerName, string fileName)
            => _storage.HasFile(pathOrContainerName, fileName);
    }
}

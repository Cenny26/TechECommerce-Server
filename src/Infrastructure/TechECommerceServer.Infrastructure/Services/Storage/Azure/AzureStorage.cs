using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TechECommerceServer.Application.Abstractions.Storage.Azure;

namespace TechECommerceServer.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private BlobContainerClient _blobContainerClient;
        public AzureStorage(IConfiguration configuration)
        {
            // reading the required connection string value for connect to the Azure Portal acc.
            _blobServiceClient = new BlobServiceClient(configuration["Storage:AzureStorage"]);
        }                        

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadFileAsync(string containerName, IFormFileCollection files)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<(string fileName, string pathOrContainerName)> data = new List<(string fileName, string pathOrContainerName)>();
            foreach (IFormFile file in files)
            {
                string newFileName = await FileRenameAsync(containerName, file.Name, HasFile);

                BlobClient blobClient = _blobContainerClient.GetBlobClient(newFileName);
                await blobClient.UploadAsync(file.OpenReadStream());

                data.Add((newFileName, $"{containerName}\\{newFileName}"));
            }

            return data;
        }

        public async Task DeleteFileAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }        

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(item => item.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(item => item.Name == fileName);
        }
    }
}

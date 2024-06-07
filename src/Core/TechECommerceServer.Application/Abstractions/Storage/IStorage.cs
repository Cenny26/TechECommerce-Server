using Microsoft.AspNetCore.Http;

namespace TechECommerceServer.Application.Abstractions.Storage
{
    public interface IStorage
    {
        Task<List<(string fileName, string pathOrContainerName)>> UploadFileAsync(string pathOrContainerName, IFormFileCollection files);
        Task DeleteFileAsync(string pathOrContainerName, string fileName);
        List<string> GetFiles(string pathOrContainerName);
        bool HasFile(string pathOrContainerName, string fileName);
    }
}

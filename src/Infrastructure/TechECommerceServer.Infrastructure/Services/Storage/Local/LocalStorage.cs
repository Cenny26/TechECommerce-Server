using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TechECommerceServer.Application.Abstractions.Storage.Local;
using TechECommerceServer.Infrastructure.Operations.File;

namespace TechECommerceServer.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        private readonly string _basePath = "wwwroot";
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadFileAsync(string path, IFormFileCollection files)
        {
            // example path: wwwroot\\resource\\files\\product\\photo-images
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string pathOrContainerName)> data = new List<(string fileName, string pathOrContainerName)>();
            foreach (IFormFile file in files)
            {
                string newFileName = await FileRenameAsync(uploadPath, file.Name, HasFile);

                await CopyOperation.CopyFileAsync($"{uploadPath}\\{newFileName}", file);
                data.Add((newFileName, $"{path}\\{newFileName}"));
            }

            // todo : if the above if is not valid, a warning exception must be created and thrown stating that an error is received while loading as files as a result!
            return data;
        }

        public async Task DeleteFileAsync(string path, string fileName)
            => await Task.Run(() =>
            {
                if (File.Exists(Path.Combine(_basePath, path)))
                    File.Delete(Path.Combine(_basePath, path));
            });

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            return directory.GetFiles().Select(file => file.Name).ToList();
        }

        public bool HasFile(string path, string fileName)
             => File.Exists($"{path}\\{fileName}");
    }
}

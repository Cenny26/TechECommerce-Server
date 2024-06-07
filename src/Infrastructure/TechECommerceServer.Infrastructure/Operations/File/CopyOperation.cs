using Microsoft.AspNetCore.Http;

namespace TechECommerceServer.Infrastructure.Operations.File
{
    public static class CopyOperation
    {
        public static async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();

                return true;
            }
            catch (Exception exc)
            {
                // todo: need to configure any log structure!
                throw new Exception("Unexpected error encountered while loading file(s)");
            }
        }
    }
}

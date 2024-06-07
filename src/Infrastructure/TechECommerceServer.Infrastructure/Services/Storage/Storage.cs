using TechECommerceServer.Infrastructure.Operations.File;

namespace TechECommerceServer.Infrastructure.Services.Storage
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);

        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileDelegate)
        {
            return await Task.Run<string>(() =>
            {
                string oldFileName = Path.GetFileNameWithoutExtension(fileName);
                string extension = Path.GetExtension(fileName);
                string newFileName = $"{NameOperation.CharacterRegulatory(oldFileName)}{extension}";

                int fileIndex = 1;
                while (hasFileDelegate(pathOrContainerName, newFileName))
                {
                    fileIndex++;
                    newFileName = $"{NameOperation.CharacterRegulatory(oldFileName)}-{fileIndex}{extension}";
                }

                return newFileName;
            });
        }
    }
}

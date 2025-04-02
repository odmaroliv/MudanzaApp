namespace MudanzaApp.Data.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(byte[] fileBytes, string fileName, string contentType);
        Task<bool> DeleteFileAsync(string fileUrl);
    }
}

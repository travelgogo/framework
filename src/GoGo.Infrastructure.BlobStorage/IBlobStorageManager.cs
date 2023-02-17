namespace GoGo.Infrastructure.BlobStorage
{
    public interface IBlobStorageManager
    {
        string GrantAccess(string fileName, int durationInSecond = 300);
        string GrantAccess(int durationInSecond = 300);
        Task<string> UploadBlobAsync(string fileName, byte[] fileContent, string? contentType = null);
        Task<string> CopyBlobAsync(string fileNameSource, string newPathWithFileName);
    }
}
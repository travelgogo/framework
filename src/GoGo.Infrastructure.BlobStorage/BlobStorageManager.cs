using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace GoGo.Infrastructure.BlobStorage
{
    class BlobStorageManager : IBlobStorageManager
    {
        private readonly CloudBlobContainer _container;
        private readonly CloudStorageAccount _storageAccount;

        public BlobStorageManager(BlobStorageOptions option, string? container)
        {
            container ??= option.DefaultContainer;
            string host = "DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2};EndpointSuffix={3}";
            host = string.Format(host,
                option.DefaultEndpointsProtocol,
                option.AccountName,
                option.AccountKey,
                option.EndpointSuffix);

            _storageAccount = CloudStorageAccount.Parse(host);
            var client = _storageAccount.CreateCloudBlobClient();
            _container = client.GetContainerReference(container);
            _container.CreateIfNotExists();
        }

        public string GrantAccess(string fileName, int durationInSecond = 300)
        {
            SharedAccessBlobPolicy policy = new()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddSeconds(durationInSecond),
            };
            var blobPath = _container.GetBlockBlobReference(fileName);
            var sasToken = blobPath.GetSharedAccessSignature(policy);
            return sasToken;
        }

        public string GrantAccess(int durationInSecond = 300)
        {
            SharedAccessAccountPolicy policy = new()
            {
                Permissions = SharedAccessAccountPermissions.Read,
                Services = SharedAccessAccountServices.Blob,
                ResourceTypes = SharedAccessAccountResourceTypes.Object,
                SharedAccessExpiryTime = DateTime.UtcNow.AddSeconds(durationInSecond),
                Protocols = SharedAccessProtocol.HttpsOnly
            };
            return _storageAccount.GetSharedAccessSignature(policy);
        }

        public async Task<string> UploadBlobAsync(string fileName, byte[] fileContent, string? contentType = null)
        {
            if (fileContent == null || fileContent.Length == 0)
                throw new Exception("FileContent can't be null.");

            var blockBlob = _container.GetBlockBlobReference(fileName);

            await blockBlob.UploadFromByteArrayAsync(fileContent, 0, fileContent.Length);

            if (contentType != null)
            {
                blockBlob.Properties.ContentType = contentType;
                await blockBlob.SetPropertiesAsync();
            }
            return blockBlob.Uri.AbsoluteUri;
        }

        public async Task<string> CopyBlobAsync(string fileNameSource, string newPathWithFileName)
        {
            if (fileNameSource.Equals(newPathWithFileName))
                throw new Exception("fileNameSource can't be the same with newPathWithFileName.");

            if (string.IsNullOrEmpty(fileNameSource) || string.IsNullOrEmpty(newPathWithFileName))
                throw new Exception("fileNameSource & newPathWithFileName can't be null.");

            var sourceBlob = _container.GetBlockBlobReference(fileNameSource);
            var descBlob = _container.GetBlockBlobReference(newPathWithFileName);
            await descBlob.StartCopyAsync(sourceBlob);
            return newPathWithFileName;
        }
    }
}
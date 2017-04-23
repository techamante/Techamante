using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;

namespace Techamante.Storage
{
    public class AzureBlobProvider
    {
        private readonly CloudStorageAccount _cloudStorageAccount;

        public AzureBlobProvider(string storageConnectionString)
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);
        }

        private CloudBlobContainer CreateContainerIfnotExists(string containerName)
        {       
            var blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            return container;
        }

        public async Task<string> UploadStreamAsync(string containerName, string filename, string contentType, Stream stream)
        {

            var container = CreateContainerIfnotExists(containerName);

            // Retrieve reference to a blob named "myblob".
            var blockBlob = container.GetBlockBlobReference(filename);

            // Create or overwrite the blob with the passed data.
            stream.Seek(0, SeekOrigin.Begin);
            await blockBlob.UploadFromStreamAsync(stream);
            stream.Close();
            stream.Dispose();
            //blockBlob.Properties.ContentEncoding = contentEncoding;
            blockBlob.Properties.ContentType = contentType;
            //blockBlob.Properties.ContentDisposition = contentDisposition;
            blockBlob.SetProperties();
            return blockBlob.Uri.ToString();
        }

        public async Task<string> UploadFileAsync(string containerName, string filename, string originalFilename, string contentType, byte[] data)
        {
            // Create or overwrite the blob with the passed data.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await memoryStream.WriteAsync(data, 0, data.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return await UploadStreamAsync(containerName, filename, contentType, memoryStream);
            }
        }

        public async Task DeleteAsync(string containerName, string fileUri)
        {
            var container = CreateContainerIfnotExists(containerName);		
            var blockBlob = container.GetBlockBlobReference(fileUri);
            try
            {
                await blockBlob.DeleteAsync();
            }
            catch (Exception ex)
            {
                // log??
            }
        }
    }
}

using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace GasB360_server.Services;

public interface IAzureStorage
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
}

public class AzureStorage : IAzureStorage
{
    private readonly string _storageConnectionString;

    public AzureStorage(IConfiguration configuration)
    {
        _storageConnectionString = configuration.GetConnectionString("AzureBlobStorage");
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var container = new BlobContainerClient(_storageConnectionString, "customer-profile-image");
        var createResponse = await container.CreateIfNotExistsAsync();

        if (createResponse != null && createResponse.GetRawResponse().Status == 201)
            await container.SetAccessPolicyAsync(PublicAccessType.Blob);

        var blob = container.GetBlobClient(fileName);
        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

        await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });

        return blob.Uri.ToString();
    }
}

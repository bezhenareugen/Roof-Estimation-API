namespace RoofEstimation.BLL.Services.MinioService;

public interface IMinioService
{
    Task<string> GetPresignedUrl(string bucketName, string objectName,
        CancellationToken cancellationToken = default);
}
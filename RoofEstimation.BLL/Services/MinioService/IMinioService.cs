using System.Security.Claims;

namespace RoofEstimation.BLL.Services.MinioService;

public interface IMinioService
{
    Task<string> GetPresignedUrl(string bucketName, string objectName,
        CancellationToken cancellationToken = default);

    Task<bool> UploadEstimation(ClaimsPrincipal user);
}
using System.Security.Claims;
using RoofEstimation.Models.Files;

namespace RoofEstimation.BLL.Services.MinioService;

public interface IMinioService
{
    Task<string> GetPresignedUrl(string bucketName, string objectName,
        CancellationToken cancellationToken = default);

    Task<bool> UploadEstimation(ClaimsPrincipal user);
    Task<List<EstimationFile>> GetUserEstimationsAsync(ClaimsPrincipal user);

    Task<string> GetLogoAsSvg(string bucketName, string objectName, CancellationToken cancellationToken = default);
}
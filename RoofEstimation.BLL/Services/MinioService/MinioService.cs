using Minio;
using Minio.DataModel.Args;

namespace RoofEstimation.BLL.Services.MinioService;

public class MinioService(IMinioClientFactory minioClientFactory) : IMinioService
{
     public async Task<string> GetPresignedUrl(string bucketName, string objectName, CancellationToken cancellationToken = default)
     {
          var minioClient = minioClientFactory.CreateClient();
            
          var statObjectArgs = new StatObjectArgs()
               .WithBucket(bucketName)
               .WithObject(objectName);
          await minioClient.StatObjectAsync(statObjectArgs, cancellationToken);


          var args = new PresignedGetObjectArgs()
               .WithBucket(bucketName)
               .WithObject(objectName)
               .WithExpiry(1200);
            
          var url = await minioClient.PresignedGetObjectAsync(args);
          return url;
     }
}
using System.Security.Claims;
using System.Text;
using CommunityToolkit.HighPerformance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Minio;
using Minio.DataModel.Args;
using RoofEstimation.Entities.Auth;

namespace RoofEstimation.BLL.Services.MinioService;

public class MinioService(IMinioClientFactory minioClientFactory, UserManager<UserEntity> userManager) : IMinioService
{
     private readonly IMinioClient _minioClient = minioClientFactory.CreateClient();

     public async Task<string> GetPresignedUrl(string bucketName, string objectName, CancellationToken cancellationToken = default)
     {
          var statObjectArgs = new StatObjectArgs()
               .WithBucket(bucketName)
               .WithObject(objectName);
          await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);
          
          var args = new PresignedGetObjectArgs()
               .WithBucket(bucketName)
               .WithObject(objectName)
               .WithExpiry(1200);
            
          var url = await _minioClient.PresignedGetObjectAsync(args);
          return url;
     }

     public async Task<bool> UploadEstimation(ClaimsPrincipal user)
     {
          var userEmail = userManager.GetUserId(user!);
          // Get the current year and month
          var currentDate = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
          
          var content = "Dummy Text File for Testing\nThis is a sample in-memory text file created for testing purposes.\nCreated on: " + DateTime.Now;

          // Convert the content to a byte array
          var contentBytes = Encoding.UTF8.GetBytes(content);

          // Define the object name with year, month, and PDF details
          var objectName = $"users/{userManager.GetUserId(user!)}/{currentDate}/dummy_test.txt";

          try
          {
               // Create a ReadOnlyMemory<byte> for the content
               ReadOnlyMemory<byte> bs = new ReadOnlyMemory<byte>(contentBytes);

               Console.WriteLine("Running example for API: PutObjectAsync");

               // Convert ReadOnlyMemory to a stream
               await using var filestream = bs.AsStream();

               var metaData = new Dictionary<string, string>
               {
                    { "Test-Metadata", "Test  Test" }
               };

               var args = new PutObjectArgs()
                    .WithBucket("estimations")
                    .WithObject(objectName)
                    .WithStreamData(filestream)
                    .WithObjectSize(filestream.Length)
                    .WithContentType("application/octet-stream")
                    .WithHeaders(metaData);

               // Upload the object to MinIO
               var response  = await _minioClient.PutObjectAsync(args).ConfigureAwait(false);

               return response.ResponseStatusCode == System.Net.HttpStatusCode.OK;
          }
          catch (Exception e)
          {
               Console.WriteLine($"[Bucket]  Exception: {e}");
               return false;
          }
     }
}
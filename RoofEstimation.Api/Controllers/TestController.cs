using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using RoofEstimation.DAL;
using System.IO;
using RoofEstimation.BLL.Services.MinioService;

namespace RoofEstimation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(ApplicationDbContext context, IMinioService minioService) : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {    
        var test = context.Users.FirstOrDefault();
        return Ok(test);
    }
    
    // [HttpGet]
    // [Route("minio")]
    // public async Task<IActionResult> GetUrl(string bucketID, CancellationToken ct)
    // {
    //     // try
    //     // {
    //     //     var minioClient = minioClientFactory.CreateClient();
    //     //     var listArgs = new ListObjectsArgs()
    //     //         .WithBucket(bucketID)
    //     //         .WithRecursive(true);
    //     //     var response = minioClient.ListObjectsEnumAsync(listArgs, ct);
    //     //     
    //     //     List<string> result = [];
    //     //
    //     //     await foreach (var item in response)
    //     //     {
    //     //         result.Add($"Object: {item.Key}, Size: {item.Size} bytes");
    //     //     }
    //     //     return Ok(result);
    //     // }
    //     // catch (Exception e)
    //     // {
    //     //     Console.WriteLine(e);
    //     //     throw;
    //     // }
    // }
    
    [HttpGet]
    [Route("minio/download")]
    public async Task<IActionResult> GetObject(string bucketId, string objectName, CancellationToken ct)
    {
        var url = await minioService.GetPresignedUrl(bucketId, objectName, ct);
        
        return string.IsNullOrEmpty(url) ? NotFound() : Ok(url);
    }
}
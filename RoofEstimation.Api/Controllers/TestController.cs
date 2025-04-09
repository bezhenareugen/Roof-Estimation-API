using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using RoofEstimation.DAL;
using System.IO;
using Microsoft.AspNetCore.Identity;
using RoofEstimation.BLL.Services.MailService;
using RoofEstimation.BLL.Services.MinioService;
using RoofEstimation.BLL.Services.PdfService;
using RoofEstimation.Entities.Auth;

namespace RoofEstimation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(ApplicationDbContext context, IMinioService minioService, IPdfService pdfService, IMailService mailService, IWebHostEnvironment env, UserManager<UserEntity> userManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var test = context.Users.FirstOrDefault();

        try
        {
            var t = context.TearOffs.ToList();
            if (t != null)
            {
               
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            Console.WriteLine($"Stack Trace: {e.StackTrace}");
            throw;
        }
        
        var templatePath = Path.Combine(env.ContentRootPath, "EmailTemplates", "WelcomeEmailTemplate.html");
        var htmlTemplate = await System.IO.File.ReadAllTextAsync(templatePath);
        
        var userId  = userManager.GetUserId(User);
        var user = await userManager.FindByEmailAsync(userId);
        
        await mailService.SendEmailAsync("eugenbezhenar@gmail.com", "Welcome to RoofEst", htmlTemplate, user!, null);

        return Ok("WebHoos Works");
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

    [HttpGet("testPdf")]
    public IActionResult GetTestPdf()
    {
        return File(pdfService.GeneratePdfAsync(), "application/pdf", "test.pdf");
    }
    
    [HttpGet]
    [Authorize]
    [Route("minio/download")]
    public async Task<IActionResult> GetObject(string bucketId, string objectName, CancellationToken ct)
    {
        var url = await minioService.GetPresignedUrl(bucketId, objectName, ct);

        var response = await minioService.UploadEstimation(User);

        return response ? Ok(url) : BadRequest("File not saved");
    }
}
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
using RoofEstimation.Models.Emails;
using RoofEstimation.Models.Enums;

namespace RoofEstimation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(ApplicationDbContext context, IMinioService minioService, IPdfService pdfService, IMailService mailService, IWebHostEnvironment env, UserManager<UserEntity> userManager, IPupetteerPdfService pupetteerPdfService) : ControllerBase
{
    [HttpGet]
    [Authorize]
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
        
        // var templatePath = Path.Combine(env.ContentRootPath, "EmailTemplates", "WelcomeEmailTemplate.html");
        // var htmlTemplate = await System.IO.File.ReadAllTextAsync(templatePath);
        
        var userId  = userManager.GetUserId(User);
        var user = await userManager.FindByEmailAsync(userId);

        var sendEmailParams = new SendEmail
        {
            ToName = "Tuzic Sharic",
            ToEmailAddress = "cto@roof-est.com",
            Subject = "Welcome to RoofEst",
            Body = string.Empty,
            EmailType = EmailType.WelcomeEmail,
        };
        
        await mailService.SendEmailAsync(sendEmailParams);

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
    
    [HttpGet("pdf")]
    public async Task<IActionResult> TestPdfPupetteer()
    {
        var pdfBytes = await pupetteerPdfService.GeneratePdfFromHtmlAsync(new EstimationData());

        if (pdfBytes.Length > 0)
        {
            return File(pdfBytes, "application/pdf", "estimation.pdf"); // Return the PDF as a file
        }
        else
        {
            return StatusCode(500, "Failed to generate PDF."); // Handle the case where PDF generation fails
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoofEstimation.BLL.Services.MinioService;

namespace RoofEstimation.Api.Controllers.Minio;

[ApiController]
[Route("api/[controller]")]
public class FilesController(IMinioService minioService) : ControllerBase
{
     [HttpGet("getEstimations")]
     [Authorize]
     public async Task<IActionResult> GetEstimations()
     {
         return Ok(await minioService.GetUserEstimationsAsync(User));
     }
}
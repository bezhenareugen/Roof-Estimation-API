using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoofEstimation.DAL;
using RoofEstimation.Models.Prices;

namespace RoofEstimation.Api.Controllers.Admin.Prices;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class GuttersController(ApplicationDbContext context) : ControllerBase
{
   [HttpGet("getGutters")]
   public async Task<IActionResult> GetGutters()
   {
      var gutters = await context.Gutters.ToListAsync();

      return Ok(gutters);
   }
   
   [HttpPatch("updateGutterPrice")]
   public async Task<IActionResult> UpdateGutterPrice([FromBody] PriceUpdateRequest gutter)
   {
      var gutterToUpdate = await context.Gutters.FindAsync(gutter.Id);
      
      if (gutterToUpdate == null)
      {
         return BadRequest(new { Errors = new List<string> { "Gutter not found" } });
      }

      gutterToUpdate.Price= gutter.Price;
      context.Gutters.Update(gutterToUpdate);
      await context.SaveChangesAsync();
      
      return Ok(new { Success = true });
   }
}
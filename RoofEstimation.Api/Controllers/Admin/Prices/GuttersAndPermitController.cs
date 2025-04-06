using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoofEstimation.DAL;
using RoofEstimation.Models.Prices;

namespace RoofEstimation.Api.Controllers.Admin.Prices;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class GuttersAndPermitController(ApplicationDbContext context) : ControllerBase
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
   
   [HttpGet("getPermits")]
   public async Task<IActionResult> GetPermits()
   {
      var permits = await context.PermitFees.ToListAsync();

      return Ok(permits);
   }
   
   [HttpPatch("updatePermitPrice")]
   public async Task<IActionResult> UpdatePermitPrice([FromBody] PriceUpdateRequest permit)
   {
      var permitToUpdate = await context.PermitFees.FindAsync(permit.Id);
      
      if (permitToUpdate == null)
      {
         return BadRequest(new { Errors = new List<string> { "Permit not found" } });
      }

      permitToUpdate.Price= permit.Price;
      context.PermitFees.Update(permitToUpdate);
      await context.SaveChangesAsync();
      
      return Ok(new { Success = true });
   }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoofEstimation.DAL;
using RoofEstimation.Models.Prices;

namespace RoofEstimation.Api.Controllers.Admin.Prices;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class MaterialsController(ApplicationDbContext dbContext) : ControllerBase
{
     [HttpGet]
     [Route("getMaterials")]
     public async Task<IActionResult> GetMaterials()
     {
         var materials = await dbContext.Materials.ToListAsync();
         
         return Ok(materials);
     }
     
     [HttpPatch]
     [Route("updateMaterialPrice")]
     public async Task<IActionResult> UpdateMaterialPrice([FromBody] MaterialPriceUpdateRequest material)
     {
         var materialToUpdate = await dbContext.Materials.FindAsync(material.Id);
         
         if (materialToUpdate == null)
         {
             return BadRequest(new { Errors = new List<string> { "Material not found" } });
         }

         materialToUpdate.MaterialPrice = material.Price;
         dbContext.Materials.Update(materialToUpdate);
         await dbContext.SaveChangesAsync();
         
         return Ok(new { Success = true });
     }
}
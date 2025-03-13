using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoofEstimation.Entities;
using RoofEstimation.Entities.Auth;
using RoofEstimation.Entities.Gutters;
using RoofEstimation.Entities.Labor;
using RoofEstimation.Entities.Material;
using RoofEstimation.Entities.PermitFees;
using RoofEstimation.Entities.PipeInfo;
using RoofEstimation.Entities.RoofInfo;
using RoofEstimation.Entities.TearOff;

namespace RoofEstimation.DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<UserEntity>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<OldPasswordEntity> OldPasswords { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<RoofInfoEntity> RoofInfos { get; set; }
    public DbSet<PipeInfoEntity> PipeInfos { get; set; }
    public DbSet<InstallLaborCostEntity> InstallLaborCosts { get; set; }
    public DbSet<MaterialEntity> Materials { get; set; }
    public DbSet<TearOffEntity> TearOffs { get; set; }
    public DbSet<GuttersEntity> Gutters { get; set; }
    public DbSet<PermitFeesEntity> PermitFees { get; set; }
}

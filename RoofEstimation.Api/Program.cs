using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Minio;
using RoofEstimation.BLL.Services.EstimationService;
using RoofEstimation.BLL.Services.LaborService;
using RoofEstimation.BLL.Services.MaterialsService;
using RoofEstimation.BLL.Services.MinioService;
using RoofEstimation.BLL.Services.TearOffService;
using RoofEstimation.DAL;
using RoofEstimation.Entities.Auth;
using RoofEstimation.Models.Configs;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("RoofEstimationDb")));

builder.Services.AddIdentityApiEndpoints<UserEntity>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//Configurations
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

var tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]!)),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    RequireExpirationTime = false,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton(tokenValidationParams);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwt =>
    {
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = tokenValidationParams;
    });

builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(builder.Configuration["MinioConfig:Endpoint"]!)
    .WithCredentials(builder.Configuration["MinioConfig:User"]!, builder.Configuration["MinioConfig:Password"]!)
    .WithSSL(false)
    .Build());

//Services
builder.Services.AddScoped<ILaborService, LaborService>();
builder.Services.AddScoped<ITearOffService, TearOffService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IEstimationService, EstimationService>();
builder.Services.AddScoped<IMinioService, MinioService>();

// var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: MyAllowSpecificOrigins,
//         policy  =>
//         {
//             policy.WithOrigins("http://localhost:3000",
//                     "http://localhost:5173")
//                 .AllowAnyHeader()
//                 .AllowAnyMethod()
//                 .AllowCredentials();
//         });
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Roof Estimation API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseStaticFiles();

//app.UseCors(MyAllowSpecificOrigins);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
using factura_api_security_project.Data;
using factura_api_security_project.Repositories;
using factura_api_security_project.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.GetSection("AzureAd");

var config2 = builder.Configuration.GetSection("AzureAd2");

//builder.Services.AddAuthentication(options =>
//{
//    // Set the default scheme for signing users in and out (Cookies)
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    // Set the default scheme for challenging unauthenticated users (OIDC redirect)
//    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
//})
//   .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
//   .EnableTokenAcquisitionToCallDownstreamApi()
//   .AddInMemoryTokenCaches();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();


//builder.Services.AddControllersWithViews()
//    .AddMicrosoftIdentityUI();

//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = options.DefaultPolicy;
//});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Register Interfaces
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();



//Set DB connection string
var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}
else
{
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(connection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/test/{nombre}", (string nombre) =>
   $"hola, {nombre}!");

app.Run();

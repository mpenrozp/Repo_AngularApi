using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiProducto.Services;
using WebApiProducto.Interfaces;
using Serilog;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using WebApiProducto.Extensions;
using WebApiProducto.Filters;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using WebApiProducto.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Text.Json.Serialization;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using WebApiProducto.Models;



var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration).Enrich.WithProperty("ApplicationName", "WebApiProducto"));
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
/*var logg = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(LogEventLevel.Information)
                .WriteTo.File("log.txt",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true)
                .CreateLogger();
 logg.Dispose();               
*/
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer()
    .AddCustomApiVersioning();

// Add services to the container.
builder.Services.AddScoped<IProductos, ServiceProducto>();
builder.Services.AddScoped<IToken, ServiceToken>();
builder.Services.AddTransient<IServiceBus, ServiceBus>();
//builder.Services.AddHttpClient();
builder.Services.AddHttpClient("GetImagenes", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.escuelajs.co/");
    httpClient.Timeout = TimeSpan.FromSeconds(5);
    // ...
});
// Add FV
builder.Services.AddFluentValidationAutoValidation();
// Add FV validators
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddDbContext<ApplicationDbContext>(opciones => 
    opciones.UseSqlServer("name=DefaultConnection"));

    
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddJsonOptions(opciones => 
    opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddXmlDataContractSerializerFormatters();
    
var app = builder.Build();

app.UseCors(options =>
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

var apiVersionDescriptionProvider =
    app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        // Allows multiple versions of our routes.
        // .Reverse(), first shown most recent version.
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            config.SwaggerEndpoint(
                url: $"/swagger/{description.GroupName}/swagger.json",
                name: $"Web API Producto- {description.GroupName.ToUpper()}");
        }
    });

//}
//if (app.Environment.IsProduction())
//{
//builder.Configuration.AddAzureKeyVault(
//new Uri(builder.Configuration["Secrets:UriKeyVault"]),
//new DefaultAzureCredential(new DefaultAzureCredentialOptions {  ManagedIdentityClientId = Environment.GetEnvironmentVariable("client-id") }),
//new AzureKeyVaultConfigurationOptions()
//{
//    ReloadInterval = TimeSpan.FromSeconds(int.Parse(builder.Configuration["Secrets:ReloadInterval"]))
//});
//}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.Run();

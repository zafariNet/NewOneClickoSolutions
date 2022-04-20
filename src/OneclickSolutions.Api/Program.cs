using OnclickSolutions.AutoClick.DataAccess;
using OneclickSolutions.Api;
using OneClickSolutions.Application;
using OneClickSolutions.Application.Configuration;
using OneClickSolutions.Domain.DocumentTypes.Catalogs;
using OneClickSolutions.Domain.Service;
using OneClickSolutions.Infrastructure;
using OneClickSolutions.Infrastructure.Domain;
using OneClickSolutions.Infrastructure.Exceptions;
using OneClickSolutions.Infrastructure.FluentValidation;
using OneClickSolutions.Infrastructure.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var services = builder.Services;
var _configuration = builder.Configuration;
services.Configure<ProjectOptions>(_configuration.Bind);
services.Configure<ExceptionOptions>(_configuration.GetSection("Exception"));

services.AddFramework()
    .WithModelValidation()
    .WithFluentValidation()
    .WithMemoryCache()
    .WithSecurityService()
    .WithBackgroundTaskQueue()
    .WithRandomNumber();

services.AddWebFramework()
    .WithPermissionAuthorization()
    .WithProtection()
    .WithPasswordHashAlgorithm()
    .WithQueuedHostedService()
    .WithAntiXsrf()
    .WithEnvironmentPath();

services.AddInfrastructure(_configuration);
services.AddApplication();
services.AddWebApp();
services.AddDomainPolicies();
services.AddJwtAuthentication(_configuration);

//services.AddTransient<IDomainEventHandler<DocumentTypeCreated>, DocumentTypeCreatedEventHandler>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

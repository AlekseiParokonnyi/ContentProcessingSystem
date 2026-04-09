using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using FileProcessingApplication;
using FileProcessingApplication.Models;
using FileProcessingCore.IPublisher;
using FileProcessingCore.IRepositories;
using FileProcessingCore.IStorage;
using FileProcessingPublisher;
using FileProcessingRepository;
using FileProcessingStorage;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<FilesDbContext>(options =>
  options.UseNpgsql(
    builder.Configuration.GetConnectionString("Postgres")));

builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection("Storage"));

builder.Services.AddSingleton(sp =>
{
  var options = sp.GetRequiredService<IOptions<StorageOptions>>().Value;
  var client = new BlobContainerClient(new Uri($"https://{options.AccountName}.blob.core.windows.net/{options.ContainerName}"), new DefaultAzureCredential());
  return client;
});

builder.Services.Configure<BusOptions>(builder.Configuration.GetSection("ServiceBus"));

builder.Services.AddSingleton<ServiceBusClient>(sp =>
{
  var options = sp.GetRequiredService<IOptions<BusOptions>>().Value;
  return new ServiceBusClient(options.FullyQualifiedNamespace, new DefaultAzureCredential());
});

builder.Services.AddSingleton<ServiceBusSender>(sp =>
{
  var options = sp.GetRequiredService<IOptions<BusOptions>>().Value;
  var client = sp.GetRequiredService<ServiceBusClient>();
  return client.CreateSender(options.TopicName);
});

builder.Services.AddScoped<IFileInfoRepository, FileInfoRepository>();
builder.Services.AddScoped<IFilesStorage, FilesStorage>();
builder.Services.AddScoped<IBusPublisher, BusPublisher>();
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var filesApi = app.MapGroup("/files");

filesApi.MapPost("", async (IFormFile file,
  IFileProcessingService fileProcessingService,
  CancellationToken ct) =>
{
  if (file is not { Length: not 0 })
    return Results.BadRequest("File is empty");

  await using var stream = file.OpenReadStream();

  var result = await fileProcessingService.StoreFileAsync(file.FileName, stream, ct);

  return Results.Ok(result);
})
.DisableAntiforgery()
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest);

filesApi.MapGet("/", (IFileProcessingService service, CancellationToken ct)
  => service.GetAllFilesAsync(ct))
  .Produces(StatusCodes.Status200OK);

filesApi.MapGet("/{id}", async Task<Results<FileStreamHttpResult, NotFound>> (
    Guid id,
    IFileProcessingService fileProcessingService,
    CancellationToken ct) =>
  {
    var file = await fileProcessingService.GetFileContentAsync(id, ct);

    if (file is null)
      return TypedResults.NotFound();

    return TypedResults.File(file.Content, "application/octet-stream", file.FileInfoModel.FileName);
  })
  .WithName("GetFile")
  .Produces(StatusCodes.Status200OK, contentType: "application/octet-stream")
  .Produces(StatusCodes.Status404NotFound);

using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<FilesDbContext>();
  await db.Database.MigrateAsync();
}

app.Run();

[JsonSerializable(typeof(IEnumerable<FileUploadResult>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}

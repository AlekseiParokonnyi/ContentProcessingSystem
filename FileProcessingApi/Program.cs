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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using FileProcessingApi;

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

builder.Services.AddHealthChecks();

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHealthChecks("/healthz");

app.MapGroup("/files").MapFilesApi();

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

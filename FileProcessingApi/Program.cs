using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;
using FileProcessingApplication;
using FileProcessingApplication.Models;
using FileProcessingCore.IRepositories;
using FileProcessingCore.IStorage;
using FileProcessingRepository;
using FileProcessingStorage;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddScoped<IFileInfoRepository, FileInfoRepository>();
builder.Services.AddScoped<IFilesStorage, FilesStorage>();
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
});

filesApi.MapGet("/", (IFileProcessingService service, CancellationToken ct)
  => service.GetAllFilesAsync(ct));

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
  db.Database.Migrate();
}

app.Run();

[JsonSerializable(typeof(IEnumerable<FileJobResult>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}

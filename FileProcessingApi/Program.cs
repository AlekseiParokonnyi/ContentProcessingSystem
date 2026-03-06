using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;
using FileProcessingApplication;
using FileProcessingApplication.Models;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

filesApi.MapGet("/{id}", async Task<Results<Ok<FileJobResult>, NotFound>> (
    Guid id,
    IFileProcessingService fileProcessingService,
    CancellationToken ct) =>
  {
    var file = await fileProcessingService.GetFileAsync(id, ct);

    return file is not null
      ? TypedResults.Ok(file)
      : TypedResults.NotFound();
  })
  .WithName("GetFile")
  .Produces<FileJobResult>()
  .Produces(StatusCodes.Status404NotFound);

app.Run();

[JsonSerializable(typeof(IEnumerable<FileJobResult>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
